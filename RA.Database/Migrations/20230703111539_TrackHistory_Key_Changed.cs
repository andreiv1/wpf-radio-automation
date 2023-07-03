using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RA.Database.Migrations
{
    /// <inheritdoc />
    public partial class TrackHistory_Key_Changed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TrackHistory",
                table: "TrackHistory");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TrackHistory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrackHistory",
                table: "TrackHistory",
                column: "DatePlayed");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TrackHistory",
                table: "TrackHistory");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TrackHistory",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrackHistory",
                table: "TrackHistory",
                column: "Id");
        }
    }
}
