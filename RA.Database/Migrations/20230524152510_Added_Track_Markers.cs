using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RA.Database.Migrations
{
    /// <inheritdoc />
    public partial class Added_Track_Markers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "EndCue",
                table: "Tracks",
                type: "double(11,5)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "NextCue",
                table: "Tracks",
                type: "double(11,5)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "StartCue",
                table: "Tracks",
                type: "double(11,5)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndCue",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "NextCue",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "StartCue",
                table: "Tracks");
        }
    }
}
