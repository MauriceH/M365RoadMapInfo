using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace M365.RoadMapInfo.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Features",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    No = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Details = table.Column<string>(nullable: true),
                    MoreInfo = table.Column<string>(nullable: true),
                    AddedToRoadmap = table.Column<DateTime>(type: "date", nullable: false),
                    LastModified = table.Column<DateTime>(type: "date", nullable: false),
                    Release = table.Column<string>(maxLength: 100, nullable: true),
                    EditType = table.Column<string>(maxLength: 30, nullable: false),
                    Status = table.Column<string>(maxLength: 30, nullable: false),
                    ValuesHash = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImportBatches",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Stamp = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportBatches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Category = table.Column<string>(maxLength: 30, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImportFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FilePath = table.Column<string>(nullable: true),
                    DataDate = table.Column<DateTime>(type: "date", nullable: false),
                    ImportBatchId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportFiles_ImportBatches_ImportBatchId",
                        column: x => x.ImportBatchId,
                        principalTable: "ImportBatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeatureTags",
                columns: table => new
                {
                    FeatureId = table.Column<Guid>(nullable: false),
                    TagId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureTags", x => new { x.FeatureId, x.TagId });
                    table.ForeignKey(
                        name: "FK_FeatureTags_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeatureTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeatureChangeSets",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FeatureId = table.Column<Guid>(nullable: false),
                    ImportFileId = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Type = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureChangeSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureChangeSets_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeatureChangeSets_ImportFiles_ImportFileId",
                        column: x => x.ImportFileId,
                        principalTable: "ImportFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeatureChanges",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FeatureChangeSetId = table.Column<Guid>(nullable: false),
                    Property = table.Column<string>(maxLength: 100, nullable: true),
                    OldValue = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureChanges_FeatureChangeSets_FeatureChangeSetId",
                        column: x => x.FeatureChangeSetId,
                        principalTable: "FeatureChangeSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeatureChanges_FeatureChangeSetId",
                table: "FeatureChanges",
                column: "FeatureChangeSetId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureChangeSets_FeatureId",
                table: "FeatureChangeSets",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureChangeSets_ImportFileId",
                table: "FeatureChangeSets",
                column: "ImportFileId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureTags_TagId",
                table: "FeatureTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportFiles_ImportBatchId",
                table: "ImportFiles",
                column: "ImportBatchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeatureChanges");

            migrationBuilder.DropTable(
                name: "FeatureTags");

            migrationBuilder.DropTable(
                name: "FeatureChangeSets");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Features");

            migrationBuilder.DropTable(
                name: "ImportFiles");

            migrationBuilder.DropTable(
                name: "ImportBatches");
        }
    }
}
