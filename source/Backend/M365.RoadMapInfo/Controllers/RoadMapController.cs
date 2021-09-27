using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using M365.RoadMapInfo.Api;
using M365.RoadMapInfo.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace M365.RoadMapInfo.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class RoadMapController : Controller
    {
        private readonly ILogger<RoadMapController> _logger;
        private readonly MainDbContext _db;

        public RoadMapController(ILogger<RoadMapController> logger, MainDbContext db)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? page, [FromQuery] int? pageSize, [FromQuery] bool? totalCount)
        {
            var queryPageSize = pageSize ?? 100;
            var queryPage = page ?? 1;
            var queryTotalCount = totalCount ?? false;
            
            
            var query = QueryFeatures().OrderBy(x => x.No);

            var results = await query
                .Skip(queryPageSize * (queryPage - 1))
                .Take(queryPageSize)
                .ToListAsync();

            var listHash = results.GenerateFeatureHashListHash();
            var dtoList = new BlockingCollection<FeatureDTO>();
            Parallel.ForEach(results, feature => dtoList.Add(CreateFeatureDTO(feature)));

            var meta = new Metadata()
            {
                Items = results.Count,
                Page = queryPage,
                PageSize = queryPageSize,
                ListHash = listHash
            };
            if (queryTotalCount)
            {
                meta.TotalCount = await _db.Features.CountAsync();
            }

            
            var result = new PaginationResult<FeatureDTO>
            {
                Items = dtoList,
                Meta = meta
            };
            return Json(result);
        }
        
        
        [HttpGet("features-hashes")]
        public async Task<IActionResult> Get()
        {
            var hashes =  await _db.Features.AsNoTracking().Select(x => x.ValuesHash).ToListAsync();
            return Json(hashes);
        }

        
        [HttpGet("features/{hash}")]
        [ResponseCache(Duration = 3600 * 24 * 10, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Get([FromRoute] string hash)
        {
            var query = QueryFeatures(true);
            var feat = await query
                .FirstOrDefaultAsync(x => x.ValuesHash == hash);
            if (feat == null) return NotFound();
            return Json(CreateFeatureDTO(feat,true));
        }
        
        [AllowAnonymous]
        [HttpGet("data-hash")]
        public async Task<IActionResult> GetDataHash([FromRoute] string hash)
        {
            var features = await _db.Features.AsNoTracking().ToListAsync();
            var dataHash = features.GenerateFeatureHashListHash();
            return Json(new {Hash=dataHash});
        }

        private IQueryable<Feature> QueryFeatures(bool withChanges = false)
        {
            var query = _db.Features
                .AsNoTracking()
                .OrderBy(x=>x.No)
                .Include(x => x.FeatureTags).ThenInclude(x => x.Tag);
            if (!withChanges) return query;
            return query.Include(x => x.Changes).ThenInclude(changeSet=>changeSet.Changes);
                
        }

        private static FeatureDTO CreateFeatureDTO(Feature feat, bool withDetails = false)
        {
            var feature = new FeatureDTO
            {
                Id = feat.Id,
                No = feat.No.ToString(),
                Description = feat.Description,
                Release = feat.Release,
                Status = feat.Status.ToString(),
                EditType = feat.EditType.ToString(),
                AddedToRoadmap = feat.AddedToRoadmap,
                LastModified = feat.LastModified,
                ValueHash = feat.ValuesHash,
                Changes = feat.Changes?.OrderByDescending(x=>x.Date).Select(x => new ChangeSetDTO
                {
                    Type = x.Type.ToString(),
                    Date = x.Date,
                    Changes = x.Changes?.Select(z => new ChangeDTO
                    {
                        Property = z.Property,
                        OldValue = z.OldValue,
                        NewValue = z.NewValue,
                    }).ToList()
                }).ToList(),
                TagCategories = feat.FeatureTags?.GroupBy(x=>x.Tag.Category).Select(x => new TagCategoryDTO
                {
                    Category = x.Key.ToString(),
                    Tags = x.Select(z=>z.Tag.Name).ToList()
                }).ToList()
            };
            if (withDetails)
            {
                feature.Details = feat.Details;
                feature.MoreInfo = feat.MoreInfo;

            }
            return feature;
        }
    }
}