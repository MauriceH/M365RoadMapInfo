using CsvHelper.Configuration.Attributes;

namespace M365.RoadMapInfo.Import
{
    public class RoadMapCsvRow
    {
        [Name("Feature ID")]
        public string FeatureID { get; set; }

        [Name("Description")]
        public string Description { get; set; }

        [Name("Details")]
        public string Details { get; set; }

        [Name("Status")]
        public string Status { get; set; }

        [Name("More Info")]
        public string MoreInfo { get; set; }

        [Name("Tags - Produkte")]
        public string TagsProducts { get; set; }

        [Name("Tags - Freigabezeitraum")]
        public string TagsReleasePeriod { get; set; }

        [Name("Tags - Plattform")]
        public string TagsPlatform { get; set; }

        [Name("Tags - Cloudinstanz")]
        public string TagsCloudInstance { get; set; }

        [Name("Added to Roadmap")]
        public string AddedToRoadmap { get; set; }

        [Name("Last Modified")]
        public string LastModified { get; set; }

        [Name("Release")]
        public string Release { get; set; }

    }
}