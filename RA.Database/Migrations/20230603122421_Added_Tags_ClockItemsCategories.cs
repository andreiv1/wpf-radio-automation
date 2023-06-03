using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RA.Database.Migrations
{
    /// <inheritdoc />
    public partial class Added_Tags_ClockItemsCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFiller",
                table: "ClockItems",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxBpm",
                table: "ClockItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "MaxDuration",
                table: "ClockItems",
                type: "time(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MaxReleaseDate",
                table: "ClockItems",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinBpm",
                table: "ClockItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "MinDuration",
                table: "ClockItems",
                type: "time(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MinReleaseDate",
                table: "ClockItems",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClockItemCategory_Tags",
                columns: table => new
                {
                    ClockItemCategoryId = table.Column<int>(type: "int", nullable: false),
                    TagValueId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClockItemCategory_Tags", x => new { x.ClockItemCategoryId, x.TagValueId });
                    table.ForeignKey(
                        name: "FK_ClockItemCategory_Tags_ClockItems_ClockItemCategoryId",
                        column: x => x.ClockItemCategoryId,
                        principalTable: "ClockItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClockItemCategory_Tags_TagValues_TagValueId",
                        column: x => x.TagValueId,
                        principalTable: "TagValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ClockItemCategory_Tags_TagValueId",
                table: "ClockItemCategory_Tags",
                column: "TagValueId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClockItems_Tracks_TrackId",
                table: "ClockItems",
                column: "TrackId",
                principalTable: "Tracks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "ClockItemCategory_Tags");

            migrationBuilder.DropColumn(
                name: "IsFiller",
                table: "ClockItems");

            migrationBuilder.DropColumn(
                name: "MaxBpm",
                table: "ClockItems");

            migrationBuilder.DropColumn(
                name: "MaxDuration",
                table: "ClockItems");

            migrationBuilder.DropColumn(
                name: "MaxReleaseDate",
                table: "ClockItems");

            migrationBuilder.DropColumn(
                name: "MinBpm",
                table: "ClockItems");

            migrationBuilder.DropColumn(
                name: "MinDuration",
                table: "ClockItems");

            migrationBuilder.DropColumn(
                name: "MinReleaseDate",
                table: "ClockItems");

            migrationBuilder.AddForeignKey(
                name: "FK_ClockItems_Tracks_TrackId",
                table: "ClockItems",
                column: "TrackId",
                principalTable: "Tracks",
                principalColumn: "Id");
        }
    }
}
