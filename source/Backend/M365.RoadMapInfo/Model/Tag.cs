using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace M365.RoadMapInfo.Model
{
    public class Tag
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        
        public TagCategory Category { get; set; }
        
        [MaxLength(100)]
        public string Name { get; set; }
        
        public virtual IList<FeatureTag> FeatureTags { get; set; }
        
    }
}