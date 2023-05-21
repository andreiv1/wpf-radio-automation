using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RA.Database.Migrations
{
    /// <inheritdoc />
    public partial class ClockItem_Separation_Fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArtistSeparation",
                table: "ClockItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TitleSeparation",
                table: "ClockItems",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArtistSeparation",
                table: "ClockItems");

            migrationBuilder.DropColumn(
                name: "TitleSeparation",
                table: "ClockItems");
        }
    }
}
