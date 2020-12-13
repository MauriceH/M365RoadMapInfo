using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace M365.RoadMapInfo.Model
{
    public class FeatureChangeSet
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        
        public Guid FeatureId { get; set; }
        public Feature Feature { get; set; }
        
        public Guid ImportFileId { get; set; }
        public ImportFile ImportFile { get; set; }
        
        public DateTime Date { get; set; }
        
        public FeatureChangeSetType Type { get; set; }
        
        public virtual IList<FeatureChange> Changes { get; set; }
        
    }
}