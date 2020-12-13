using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using M365.RoadMapInfo.Model;

namespace M365.RoadMapInfo.Api
{
    public class FeatureDTO
    {
        public Guid Id { get; set; }
        
        public string No { get; set; }
        
        public string Description { get; set; }

        public string Details { get; set; }

        public string MoreInfo { get; set; }
        
        public DateTime AddedToRoadmap { get; set; }
        
        public DateTime LastModified { get; set; }
        
        public string Release { get; set; }
        
        public string EditType { get; set; }
        
        public string Status { get; set; }
        
        public string ValueHash { get; set; }
        
        public IList<TagCategoryDTO> TagCategories { get; set; }
        
        public IList<ChangeSetDTO> Changes { get; set; }
    }
}