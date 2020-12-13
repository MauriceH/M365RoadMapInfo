using System;
using System.Collections.Generic;

namespace M365.RoadMapInfo.Api
{
    public class TagCategoryDTO
    {
        public string Category { get; set; }
        public List<string> Tags { get; set; }
    }
}