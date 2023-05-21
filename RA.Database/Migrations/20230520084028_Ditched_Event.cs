using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RA.Database.Migrations
{
    /// <inheritdoc />
    public partial class Ditched_Event : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClockItems_Events_EventId",
                table: "ClockItems");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropIndex(
                name: "IX_ClockItems_EventId",
                table: "ClockItems");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "ClockItems",
                newName: "EventType");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EstimatedEventDuration",
                table: "ClockItems",
                type: "time(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EventLabel",
                table: "ClockItems",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimatedEventDuration",
                table: "ClockItems");

            migrationBuilder.DropColumn(
                name: "EventLabel",
                table: "ClockItems");

            migrationBuilder.RenameColumn(
                name: "EventType",
                table: "ClockItems",
                newName: "EventId");

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Command = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EstimatedDuration = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ClockItems_EventId",
                table: "ClockItems",
                column: "EventId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClockItems_Events_EventId",
                table: "ClockItems",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");
        }
    }
}
