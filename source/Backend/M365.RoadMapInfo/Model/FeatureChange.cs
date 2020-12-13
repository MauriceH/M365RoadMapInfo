using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace M365.RoadMapInfo.Model
{
    public class FeatureChange
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        
        public Guid FeatureChangeSetId { get; set; }
        public FeatureChangeSet FeatureChangeSet { get; set; }
        
        [MaxLength(100)]
        public string Property { get; set; }
        
        [MaxLength]
        public string OldValue { get; set; }
        
        [MaxLength]
        public string NewValue { get; set; }
        
    }
}