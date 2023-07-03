using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RA.Database.Migrations
{
    /// <inheritdoc />
    public partial class Changed_TrackHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Artists",
                table: "TrackHistory");

            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "TrackHistory");

            migrationBuilder.DropColumn(
                name: "ISRC",
                table: "TrackHistory");

            migrationBuilder.DropColumn(
                name: "LengthPlayed",
                table: "TrackHistory");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "TrackHistory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Artists",
                table: "TrackHistory",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "TrackHistory",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ISRC",
                table: "TrackHistory",
                type: "varchar(55)",
                maxLength: 55,
                nullable: true)
            .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "LengthPlayed",
                table: "TrackHistory",
                type: "time(6)",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "TrackHistory",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
