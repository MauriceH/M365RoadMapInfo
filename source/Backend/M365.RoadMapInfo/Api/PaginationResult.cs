using System.Collections.Generic;

namespace M365.RoadMapInfo.Api
{
    public class PaginationResult<T>
    {
        public Metadata Meta { get; set; }
        
        public IEnumerable<T> Items { get; set; }
    }

    public class Metadata
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Items { get; set; }
        public int TotalCount { get; set; }
        public string ListHash { get; set; }
    }
}