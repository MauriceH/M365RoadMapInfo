using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper.Internal;
using M365.RoadMapInfo.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace M365.RoadMapInfo.Import
{
    public class RoadMapImporter
    {
        private readonly MainDbContext _dbContext;
        private readonly IOptions<ImportConfig> _config;
        private readonly ImportRowMapper _mapper = new ImportRowMapper();

        public RoadMapImporter(MainDbContext dbContext, IOptions<ImportConfig> config)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task ImportAsync()
        {
            var path = _config.Value?.CsvPath ?? throw new Exception("csv import path not configured");
            var filesInDirectory = Directory.GetFiles(path);
            var validFileNames = FilterFilesByFileNameStructure(filesInDirectory).ToList();

            var minDate = validFileNames.Min(x => x.DownloadTime);

            var importFiles = _dbContext.ImportFiles.Where(x => x.DataDate >= minDate).ToList();

            var filteredFileEntries = validFileNames
                .Where(fileInfo => importFiles.All(x => x.DataDate.Date != fileInfo.DownloadTime.Date))
                .ToList();

            if (!filteredFileEntries.Any())
            {
                return;
            }

            var importBatch = new ImportBatch
            {
                Id = Guid.NewGuid(),
                Stamp = DateTimeOffset.Now
            };
            await _dbContext.ImportBatches.AddAsync(importBatch);


            var dbTags = await _dbContext.Tags.ToListAsync();

            var features = await _dbContext.Features
                .Include(c => c.FeatureTags)
                .ThenInclude(c => c.Tag)
                .ToDictionaryAsync(c => c.No);


            foreach (var fileInfo in filteredFileEntries)
            {
                var importFile = new ImportFile
                {
                    Id = Guid.NewGuid(),
                    DataDate = fileInfo.DownloadTime,
                    FilePath = fileInfo.FilePath,
                    ImportBatch = importBatch
                };
                await _dbContext.ImportFiles.AddAsync(importFile);

                List<ImportRow> entries;
                try
                {
                    entries = await LoadImportRowsAsync(fileInfo);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                var featuresAdded = 0;
                var featuresModified = 0;
                var ttt = 0;

                foreach (var importRow in entries)
                {
                    var isNew = false;
                    if (!features.TryGetValue(importRow.FeatureID, out var feature))
                    {
                        feature = new Feature
                        {
                            Id = Guid.NewGuid(),
                            AddedToRoadmap = importRow.AddedToRoadmap,
                            FeatureTags = new List<FeatureTag>()
                        };
                        features[importRow.FeatureID] = feature;
                        await _dbContext.Features.AddAsync(feature);
                        featuresAdded++;
                        isNew = true;
                    }

                    var changeSet = new FeatureChangeSet
                    {
                        Id = Guid.NewGuid(),
                        Feature = feature,
                        ImportFile = importFile,
                        Type = isNew ? FeatureChangeSetType.Added : FeatureChangeSetType.Modified,
                        Date = fileInfo.DownloadTime,
                        Changes = new List<FeatureChange>()
                    };


                    ChangeProperty(feature, importRow, changeSet, f => f.Description, s => s.Description);
                    ChangeProperty(feature, importRow, changeSet, f => f.Details, s => s.Details);
                    ChangeProperty(feature, importRow, changeSet, f => f.Status, s => s.Status);
                    ChangeProperty(feature, importRow, changeSet, f => f.EditType, s => s.EditType, false);
                    ChangeProperty(feature, importRow, changeSet, f => f.Release, s => s.Release);
                    ChangeProperty(feature, importRow, changeSet, f => f.LastModified, s => s.LastModified, false);
                    ChangeProperty(feature, importRow, changeSet, f => f.AddedToRoadmap, s => s.AddedToRoadmap);
                    ChangeProperty(feature, importRow, changeSet, f => f.MoreInfo, s => s.MoreInfo);
                    ChangeProperty(feature, importRow, changeSet, f => f.No, s => s.FeatureID);


                    ChangeTagList(feature, dbTags, changeSet, importRow.TagsPlatform, TagCategory.Platform);
                    ChangeTagList(feature, dbTags, changeSet, importRow.TagsProducts, TagCategory.Product);
                    ChangeTagList(feature, dbTags, changeSet, importRow.TagsCloudInstance, TagCategory.CloudInstance);
                    ChangeTagList(feature, dbTags, changeSet, importRow.TagsReleasePeriod, TagCategory.ReleasePeriod);

                    if (isNew || changeSet.Changes.Any())
                    {
                        feature.CalculateValueHash();
                        if (isNew)
                        {
                            changeSet.Changes.Clear();
                        }

                        await _dbContext.FeatureChangeSets.AddAsync(changeSet);
                        ttt++;
                        if (!isNew) featuresModified++;
                    }
                }

                await _dbContext.SaveChangesAsync();
                Console.WriteLine(
                    $"{fileInfo.DownloadTime.Date.ToShortDateString()} - added: {featuresAdded}, modified: {featuresModified}, ttt: {ttt} ");
            }
        }

        private static void ChangeProperty<T>(Feature feature, ImportRow importRow, FeatureChangeSet changeSet, Expression<Func<Feature, T>> dest,
            Func<ImportRow, T> src, bool addToChangeSet = true)
        {
            var currentValue = dest.Compile()(feature);
            var targetValue = src(importRow);
            if (Equals(currentValue, targetValue)) return;

            var mex = dest.Body as MemberExpression;
            if (mex?.Member == null) throw new ArgumentException(nameof(dest));
            mex.Member.SetMemberValue(feature, targetValue);
            if (!addToChangeSet) return;

            changeSet.Changes.Add(new FeatureChange
            {
                Id = Guid.NewGuid(),
                FeatureChangeSet = changeSet,
                Property = mex.Member.Name,
                OldValue = currentValue?.ToString(),
                NewValue = targetValue?.ToString()
            });
        }

        private void ChangeTagList(Feature feature, List<Tag> tags, FeatureChangeSet changeSet, List<string> targetTagList, TagCategory category)
        {
            var activeTagList = feature.FeatureTags.Where(x => x.Tag.Category == category).ToList();
            var orphanedList = activeTagList.ToList();

            var categoryTags = tags.Where(x => x.Category == category).ToList();

            foreach (var targetTagName in targetTagList)
            {
                var availableTag = activeTagList.FirstOrDefault(x => targetTagName.Equals(x.Tag.Name, StringComparison.InvariantCultureIgnoreCase));

                if (availableTag != null)
                {
                    orphanedList.Remove(availableTag);
                    continue;
                }

                var dbTag = categoryTags.FirstOrDefault(x => x.Name.Equals(targetTagName, StringComparison.CurrentCultureIgnoreCase));

                if (dbTag == null)
                {
                    dbTag = new Tag
                    {
                        Id = Guid.NewGuid(),
                        Category = category,
                        Name = targetTagName,
                    };
                    tags.Add(dbTag);
                }

                var featureTag = new FeatureTag
                {
                    Feature = feature,
                    Tag = dbTag
                };
                feature.FeatureTags.Add(featureTag);
                activeTagList.Add(featureTag);


                changeSet.Changes.Add(new FeatureChange
                {
                    Id = Guid.NewGuid(),
                    FeatureChangeSet = changeSet,
                    Property = $"Tag-{category}",
                    OldValue = "add",
                    NewValue = targetTagName
                });
            }

            foreach (var featureTag in orphanedList)
            {
                feature.FeatureTags.Remove(featureTag);

                changeSet.Changes.Add(new FeatureChange
                {
                    Id = Guid.NewGuid(),
                    FeatureChangeSet = changeSet,
                    Property = $"Tag-{category}",
                    OldValue = "remove",
                    NewValue = featureTag.Tag.Name
                });
            }
        }


        private async Task<List<ImportRow>> LoadImportRowsAsync(CsvFileInfo fileInfo)
        {
            var result = new List<ImportRow>();
            using var sw = new StreamReader(fileInfo.FilePath, Encoding.UTF8);
            using var csvReader = new RoadMapCsvReader(sw);
            {
                var rows = csvReader.ReadAllRowsAsync();
                await foreach (var csvRow in rows)
                {
                    result.Add(_mapper.Map(csvRow));
                }
            }
            return result;
        }

        private IEnumerable<CsvFileInfo> FilterFilesByFileNameStructure(IEnumerable<string> fileNames)
        {
            var regex = new Regex(@"^[A-Za-z]:(?=\\.+)*\\.+_(?<Date>\d{1,2}-\d{1,2}-\d{4})\.csv");
            foreach (var filePath in fileNames)
            {
                var match = regex.Match(filePath);
                if (!match.Success) continue;
                if (!match.Groups["Date"].Success) continue;
                var fileDateString = match.Groups["Date"].Value;
                if (!DateTime.TryParseExact(fileDateString, "MM-dd-yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out var date))
                    continue;
                yield return new CsvFileInfo
                {
                    FilePath = filePath,
                    DownloadTime = date
                };
            }
        }
    }
}