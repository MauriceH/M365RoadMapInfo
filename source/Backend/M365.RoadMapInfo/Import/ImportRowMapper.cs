using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMapper;
using AutoMapper.Features;
using M365.RoadMapInfo.Model;

namespace M365.RoadMapInfo.Import
{
    public class ImportRowMapper
    {
        private readonly IMapper _mapper;

        public ImportRowMapper()
        {
            var config = new MapperConfiguration(builder =>
            {
                var mainMap = builder.CreateMap<RoadMapCsvRow, ImportRow>();

                mainMap.ForMember(dst => dst.FeatureID, opt =>
                    opt.MapFrom(src => int.Parse(src.FeatureID)));

                mainMap.ForMember(dst => dst.Status, opt =>
                    opt.MapFrom((src,dst) => src.Status switch
                    {
                        "In development" => FeatureStatus.InDevelopment,
                        "Cancelled" => FeatureStatus.Cancelled,
                        "Launched" => FeatureStatus.Launched,
                        "Rolling out" => FeatureStatus.RollingOut,
                        _ => FeatureStatus.Unknown,
                        }));

                mainMap.ForMember(dst => dst.EditType, opt =>
                    opt.MapFrom(src =>
                        string.Equals(src.AddedToRoadmap, src.LastModified, StringComparison.InvariantCulture)
                            ? FeatureEditType.New
                            : FeatureEditType.Modified));

                mainMap.ForMember(dst => dst.AddedToRoadmap, opt =>
                    opt.MapFrom(src => DateTime.ParseExact(src.AddedToRoadmap, "yyyy-MM-dd", CultureInfo.CurrentCulture)));

                mainMap.ForMember(dst => dst.LastModified, opt =>
                    opt.MapFrom(src => DateTime.ParseExact(src.LastModified, "yyyy-MM-dd", CultureInfo.CurrentCulture)));

                mainMap.ForMember(dst => dst.TagsProducts, opt =>
                    opt.MapFrom(src => SplitTags(src.TagsProducts)));

                mainMap.ForMember(dst => dst.TagsPlatform, opt =>
                    opt.MapFrom(src => SplitTags(src.TagsPlatform)));

                mainMap.ForMember(dst => dst.TagsCloudInstance, opt =>
                    opt.MapFrom(src => SplitTags(src.TagsCloudInstance)));

                mainMap.ForMember(dst => dst.TagsReleasePeriod, opt =>
                    opt.MapFrom(src => SplitTags(src.TagsReleasePeriod)));
            });
            _mapper = config.CreateMapper();
        }

        private List<string> SplitTags(string tagContent)
        {
            return string.IsNullOrWhiteSpace(tagContent) ? new List<string>() : tagContent.Split(",").Select(x => x.Trim()).ToList();
        }


        public ImportRow Map(RoadMapCsvRow csvRow)
        {
            return _mapper.Map<ImportRow>(csvRow);
        }
    }
}