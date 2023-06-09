using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RA.Database.Migrations
{
    /// <inheritdoc />
    public partial class ClockItemEvent_Order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClockItemEventId",
                table: "ClockItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EventOrderIndex",
                table: "ClockItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClockItems_ClockItemEventId",
                table: "ClockItems",
                column: "ClockItemEventId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClockItems_ClockItems_ClockItemEventId",
                table: "ClockItems",
                column: "ClockItemEventId",
                principalTable: "ClockItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClockItems_ClockItems_ClockItemEventId",
                table: "ClockItems");

            migrationBuilder.DropIndex(
                name: "IX_ClockItems_ClockItemEventId",
                table: "ClockItems");

            migrationBuilder.DropColumn(
                name: "ClockItemEventId",
                table: "ClockItems");

            migrationBuilder.DropColumn(
                name: "EventOrderIndex",
                table: "ClockItems");
        }
    }
}
