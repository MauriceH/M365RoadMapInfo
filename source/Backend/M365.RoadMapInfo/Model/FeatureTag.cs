using System;

namespace M365.RoadMapInfo.Model
{
    public class FeatureTag
    {
        public Guid FeatureId { get; set; }
        public Feature Feature { get; set; }
        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}