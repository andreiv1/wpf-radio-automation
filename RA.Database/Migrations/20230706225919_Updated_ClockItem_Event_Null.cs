using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RA.Database.Migrations
{
    /// <inheritdoc />
    public partial class Updated_ClockItem_Event_Null : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClockItems_ClockItems_ClockItemCategoryId",
                table: "ClockItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ClockItems_ClockItems_ClockItemTrackId",
                table: "ClockItems");

            migrationBuilder.DropIndex(
                name: "IX_ClockItems_ClockItemCategoryId",
                table: "ClockItems");

            migrationBuilder.DropIndex(
                name: "IX_ClockItems_ClockItemTrackId",
                table: "ClockItems");

            migrationBuilder.DropColumn(
                name: "ClockItemCategoryId",
                table: "ClockItems");

            migrationBuilder.DropColumn(
                name: "ClockItemTrackId",
                table: "ClockItems");

            migrationBuilder.AlterColumn<int>(
                name: "ClockItemEventId",
                table: "ClockItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ClockItemEventId",
                table: "ClockItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClockItemCategoryId",
                table: "ClockItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClockItemTrackId",
                table: "ClockItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClockItems_ClockItemCategoryId",
                table: "ClockItems",
                column: "ClockItemCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ClockItems_ClockItemTrackId",
                table: "ClockItems",
                column: "ClockItemTrackId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClockItems_ClockItems_ClockItemCategoryId",
                table: "ClockItems",
                column: "ClockItemCategoryId",
                principalTable: "ClockItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClockItems_ClockItems_ClockItemTrackId",
                table: "ClockItems",
                column: "ClockItemTrackId",
                principalTable: "ClockItems",
                principalColumn: "Id");
        }
    }
}
