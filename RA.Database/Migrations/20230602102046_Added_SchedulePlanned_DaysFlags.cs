using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RA.Database.Migrations
{
    /// <inheritdoc />
    public partial class Added_SchedulePlanned_DaysFlags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFriday",
                table: "SchedulesPlanned",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMonday",
                table: "SchedulesPlanned",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSaturday",
                table: "SchedulesPlanned",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSunday",
                table: "SchedulesPlanned",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsThursday",
                table: "SchedulesPlanned",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTuesday",
                table: "SchedulesPlanned",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsWednesday",
                table: "SchedulesPlanned",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFriday",
                table: "SchedulesPlanned");

            migrationBuilder.DropColumn(
                name: "IsMonday",
                table: "SchedulesPlanned");

            migrationBuilder.DropColumn(
                name: "IsSaturday",
                table: "SchedulesPlanned");

            migrationBuilder.DropColumn(
                name: "IsSunday",
                table: "SchedulesPlanned");

            migrationBuilder.DropColumn(
                name: "IsThursday",
                table: "SchedulesPlanned");

            migrationBuilder.DropColumn(
                name: "IsTuesday",
                table: "SchedulesPlanned");

            migrationBuilder.DropColumn(
                name: "IsWednesday",
                table: "SchedulesPlanned");
        }
    }
}
