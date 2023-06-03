using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RA.Database.Migrations
{
    /// <inheritdoc />
    public partial class Added_ClockItemEvent_Start : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClockItems_Clocks_ClockId",
                table: "ClockItems");

            migrationBuilder.AlterColumn<int>(
                name: "ClockId",
                table: "ClockItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EstimatedEventStart",
                table: "ClockItems",
                type: "time(6)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClockItems_Clocks_ClockId",
                table: "ClockItems",
                column: "ClockId",
                principalTable: "Clocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClockItems_Clocks_ClockId",
                table: "ClockItems");

            migrationBuilder.DropColumn(
                name: "EstimatedEventStart",
                table: "ClockItems");

            migrationBuilder.AlterColumn<int>(
                name: "ClockId",
                table: "ClockItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ClockItems_Clocks_ClockId",
                table: "ClockItems",
                column: "ClockId",
                principalTable: "Clocks",
                principalColumn: "Id");
        }
    }
}
