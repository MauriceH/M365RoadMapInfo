using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace M365.RoadMapInfo.Model
{
    public static class FeatureEnumerableExtensions
    {
        public static string GenerateFeatureHashListHash(this IEnumerable<Feature> features)
        {
            using var ms = new MemoryStream();
            using var sw = new StreamWriter(ms, Encoding.UTF8);
            foreach (var feature in features)
            {
                sw.Write(feature.ValuesHash);
            }
            ms.Position = 0;
            using var sha1 = SHA1.Create();
            var hash = sha1.ComputeHash(ms);
            return HashExtensions.ByteArrayToHexViaLookup32Unsafe(hash);
        }
    }
}