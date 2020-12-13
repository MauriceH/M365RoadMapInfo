using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace M365.RoadMapInfo.Model
{
    public class ImportBatch
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        
        public DateTimeOffset Stamp { get; set; }
        
        public virtual IList<ImportFile> ImportFiles { get; set; }
    }
}