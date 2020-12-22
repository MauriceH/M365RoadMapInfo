﻿// <auto-generated />
using System;
using M365.RoadMapInfo.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace M365.RoadMapInfo.Migrations
{
    [DbContext(typeof(MainDbContext))]
    partial class MainDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("M365.RoadMapInfo.Model.Feature", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("AddedToRoadmap")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Details")
                        .HasColumnType("text");

                    b.Property<string>("EditType")
                        .IsRequired()
                        .HasColumnType("character varying(30)")
                        .HasMaxLength(30);

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("date");

                    b.Property<string>("MoreInfo")
                        .HasColumnType("text");

                    b.Property<int>("No")
                        .HasColumnType("integer");

                    b.Property<string>("Release")
                        .HasColumnType("character varying(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("character varying(30)")
                        .HasMaxLength(30);

                    b.Property<string>("ValuesHash")
                        .HasColumnType("character varying(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("ValuesHash");

                    b.ToTable("Features");
                });

            modelBuilder.Entity("M365.RoadMapInfo.Model.FeatureChange", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("FeatureChangeSetId")
                        .HasColumnType("uuid");

                    b.Property<string>("NewValue")
                        .HasColumnType("text");

                    b.Property<string>("OldValue")
                        .HasColumnType("text");

                    b.Property<string>("Property")
                        .HasColumnType("character varying(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("FeatureChangeSetId");

                    b.ToTable("FeatureChanges");
                });

            modelBuilder.Entity("M365.RoadMapInfo.Model.FeatureChangeSet", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("date");

                    b.Property<Guid>("FeatureId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ImportFileId")
                        .HasColumnType("uuid");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("character varying(30)")
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.HasIndex("FeatureId");

                    b.HasIndex("ImportFileId");

                    b.ToTable("FeatureChangeSets");
                });

            modelBuilder.Entity("M365.RoadMapInfo.Model.FeatureTag", b =>
                {
                    b.Property<Guid>("FeatureId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TagId")
                        .HasColumnType("uuid");

                    b.HasKey("FeatureId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("FeatureTags");
                });

            modelBuilder.Entity("M365.RoadMapInfo.Model.ImportFile", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DataDate")
                        .HasColumnType("date");

                    b.Property<string>("FileName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ImportFiles");
                });

            modelBuilder.Entity("M365.RoadMapInfo.Model.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("character varying(30)")
                        .HasMaxLength(30);

                    b.Property<string>("Name")
                        .HasColumnType("character varying(100)")
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
#pragma warning restore 612, 618
        }
    }
}
