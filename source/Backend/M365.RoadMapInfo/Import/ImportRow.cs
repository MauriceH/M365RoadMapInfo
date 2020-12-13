using System;
using System.Collections.Generic;
using M365.RoadMapInfo.Model;

namespace M365.RoadMapInfo.Import
{
    public class ImportRow
    {
        public int FeatureID { get; set; }

        public string Description { get; set; }

        public string Details { get; set; }

        public FeatureStatus Status { get; set; }

        public string MoreInfo { get; set; }

        public List<string> TagsProducts { get; set; }

        public List<string> TagsReleasePeriod { get; set; }

        public List<string> TagsPlatform { get; set; }

        public List<string> TagsCloudInstance { get; set; }

        public DateTime AddedToRoadmap { get; set; }

        public DateTime LastModified { get; set; }

        public string Release { get; set; }
        
        public FeatureEditType EditType { get; set; }
    }
}