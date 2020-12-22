using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace M365.RoadMapInfo.Model
{
    public class ImportFile
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        
        [MaxLength]
        public string FileName { get; set; }
        
        
        public DateTime DataDate { get; set; }
        
    }
}