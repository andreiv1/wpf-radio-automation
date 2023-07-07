using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RA.Database.Migrations
{
    /// <inheritdoc />
    public partial class Added_PlaylistItem_Generation_Fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BaseClockItemId",
                table: "PlaylistItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BaseTemplateId",
                table: "PlaylistItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistItems_BaseClockItemId",
                table: "PlaylistItems",
                column: "BaseClockItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistItems_BaseTemplateId",
                table: "PlaylistItems",
                column: "BaseTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistItems_ClockItems_BaseClockItemId",
                table: "PlaylistItems",
                column: "BaseClockItemId",
                principalTable: "ClockItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistItems_Templates_BaseTemplateId",
                table: "PlaylistItems",
                column: "BaseTemplateId",
                principalTable: "Templates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistItems_ClockItems_BaseClockItemId",
                table: "PlaylistItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistItems_Templates_BaseTemplateId",
                table: "PlaylistItems");

            migrationBuilder.DropIndex(
                name: "IX_PlaylistItems_BaseClockItemId",
                table: "PlaylistItems");

            migrationBuilder.DropIndex(
                name: "IX_PlaylistItems_BaseTemplateId",
                table: "PlaylistItems");

            migrationBuilder.DropColumn(
                name: "BaseClockItemId",
                table: "PlaylistItems");

            migrationBuilder.DropColumn(
                name: "BaseTemplateId",
                table: "PlaylistItems");
        }
    }
}
