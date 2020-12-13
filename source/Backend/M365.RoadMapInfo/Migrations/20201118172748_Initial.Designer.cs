﻿// <auto-generated />
using System;
using M365.RoadMapInfo.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace M365.RoadMapInfo.Migrations
{
    [DbContext(typeof(MainDbContext))]
    [Migration("20201118172748_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("M365.RoadMapInfo.Model.Feature", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("AddedToRoadmap")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Details")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EditType")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("date");

                    b.Property<string>("MoreInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("No")
                        .HasColumnType("int");

                    b.Property<string>("Release")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("ValuesHash")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Features");
                });

            modelBuilder.Entity("M365.RoadMapInfo.Model.FeatureChange", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FeatureChangeSetId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NewValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OldValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Property")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("FeatureChangeSetId");

                    b.ToTable("FeatureChanges");
                });

            modelBuilder.Entity("M365.RoadMapInfo.Model.FeatureChangeSet", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("date");

                    b.Property<Guid>("FeatureId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ImportFileId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.HasIndex("FeatureId");

                    b.HasIndex("ImportFileId");

                    b.ToTable("FeatureChangeSets");
                });

            modelBuilder.Entity("M365.RoadMapInfo.Model.FeatureTag", b =>
                {
                    b.Property<Guid>("FeatureId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TagId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("FeatureId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("FeatureTags");
                });

            modelBuilder.Entity("M365.RoadMapInfo.Model.ImportBatch", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Stamp")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("ImportBatches");
                });

            modelBuilder.Entity("M365.RoadMapInfo.Model.ImportFile", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DataDate")
                        .HasColumnType("date");

                    b.Property<string>("FilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ImportBatchId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ImportBatchId");

                    b.ToTable("ImportFiles");
                });

            modelBuilder.Entity("M365.RoadMapInfo.Model.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("M365.RoadMapInfo.Model.FeatureChange", b =>
                {
                    b.HasOne("M365.RoadMapInfo.Model.FeatureChangeSet", "FeatureChangeSet")
                        .WithMany("Changes")
                        .HasForeignKey("FeatureChangeSetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("M365.RoadMapInfo.Model.FeatureChangeSet", b =>
                {
                    b.HasOne("M365.RoadMapInfo.Model.Feature", "Feature")
                        .WithMany("Changes")
                        .HasForeignKey("FeatureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("M365.RoadMapInfo.Model.ImportFile", "ImportFile")
                        .WithMany()
                        .HasForeignKey("ImportFileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("M365.RoadMapInfo.Model.FeatureTag", b =>
                {
                    b.HasOne("M365.RoadMapInfo.Model.Feature", "Feature")
                        .WithMany("FeatureTags")
                        .HasForeignKey("FeatureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("M365.RoadMapInfo.Model.Tag", "Tag")
                        .WithMany("FeatureTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("M365.RoadMapInfo.Model.ImportFile", b =>
                {
                    b.HasOne("M365.RoadMapInfo.Model.ImportBatch", "ImportBatch")
                        .WithMany("ImportFiles")
                        .HasForeignKey("ImportBatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
