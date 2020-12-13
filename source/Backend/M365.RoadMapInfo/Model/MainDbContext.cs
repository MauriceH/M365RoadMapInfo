using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace M365.RoadMapInfo.Model
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var statusConverter = new EnumToStringConverter<FeatureStatus>();
            var editTypeConverter = new EnumToStringConverter<FeatureEditType>();

            var featureEntity = modelBuilder
                .Entity<Feature>();

            featureEntity
                .Property(e => e.Status)
                .HasConversion(statusConverter)
                .HasMaxLength(30);

            featureEntity
                .Property(e => e.EditType)
                .HasConversion(editTypeConverter)
                .HasMaxLength(30);

            featureEntity
                .Property(e => e.AddedToRoadmap)
                .HasColumnType("date");

            featureEntity
                .Property(e => e.LastModified)
                .HasColumnType("date");

            featureEntity
                .HasIndex(e => e.ValuesHash);


            var categoryConverter = new EnumToStringConverter<TagCategory>();

            
            var tagEntity = modelBuilder
                .Entity<Tag>();

            tagEntity
                .Property(e => e.Category)
                .HasConversion(categoryConverter)
                .HasMaxLength(30);


            var featureTagEntity = modelBuilder
                .Entity<FeatureTag>();

            featureTagEntity
                .HasKey(bc => new {bc.FeatureId, bc.TagId});
            
            
            var importEntryEntity = modelBuilder
                .Entity<ImportFile>();
            
            importEntryEntity
                .Property(i => i.DataDate)
                .HasColumnType("date");
            
            
            
            var typeConverter = new EnumToStringConverter<FeatureChangeSetType>();
            
            var featureChangeSetEntity = modelBuilder
                .Entity<FeatureChangeSet>();
            
            featureChangeSetEntity
                .Property(i => i.Type)
                .HasConversion(typeConverter)
                .HasMaxLength(30);
            
            featureChangeSetEntity
                .Property(i => i.Date)
                .HasColumnType("date");

            
        }

        public virtual DbSet<Feature> Features { get; set; }

        public virtual DbSet<Tag> Tags { get; set; }

        public virtual DbSet<FeatureTag> FeatureTags { get; set; }
        
        public virtual DbSet<ImportBatch> ImportBatches { get; set; }
        
        public virtual DbSet<ImportFile> ImportFiles { get; set; }
        
        public virtual DbSet<FeatureChangeSet> FeatureChangeSets { get; set; }
        
        public virtual DbSet<FeatureChange> FeatureChanges { get; set; }
        
    }
}