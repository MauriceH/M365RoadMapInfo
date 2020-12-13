using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace M365.RoadMapInfo.Model
{
    
    public class Feature
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        
        
        public int No { get; set; }
        
        
        [MaxLength]
        public string Description { get; set; }
        
        
        [MaxLength]
        public string Details { get; set; }
        
        
        [MaxLength]
        public string MoreInfo { get; set; }
        
        
        public DateTime AddedToRoadmap { get; set; }
        
        
        public DateTime LastModified { get; set; }
        
        [MaxLength(100)]
        public string Release { get; set; }
        
        
        public FeatureEditType EditType { get; set; }
        
        
        public FeatureStatus Status { get; set; }
        
        
        [MaxLength(100)]
        public string ValuesHash { get; set; }
        
        
        public virtual IList<FeatureTag> FeatureTags { get; set; }
        
        
        public virtual IList<FeatureChangeSet> Changes { get; set; }

        
        public void CalculateValueHash()
        {
            using var ms = new MemoryStream();
            using var writer = new BinaryWriter(ms, Encoding.UTF8,true);
            writer.Write(Id.ToByteArray());
            writer.Write(No);
            writer.Write(Description);
            writer.Write(Details);
            writer.Write(MoreInfo);
            writer.Write(AddedToRoadmap.ToBinary());
            writer.Write(LastModified.ToBinary());
            writer.Write(Release);
            writer.Write((int)EditType);
            writer.Write((int)Status);
            foreach (var featureTag in FeatureTags.OrderBy(x=>x.Tag.Category).ThenBy(x=>x.Tag.Name))
            {
                writer.Write((int)featureTag.Tag.Category);
                writer.Write(featureTag.Tag.Name);
            }
            writer.Flush();
            writer.Close();
            ms.Position = 0;
            var hash = SHA1.Create().ComputeHash(ms);
            ValuesHash = HashExtensions.ByteArrayToHexViaLookup32Unsafe(hash);
        }
    }
}