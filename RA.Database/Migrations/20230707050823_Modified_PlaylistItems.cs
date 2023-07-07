using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RA.Database.Migrations
{
    /// <inheritdoc />
    public partial class Modified_PlaylistItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistItems_Tracks_TrackId",
                table: "PlaylistItems");

            migrationBuilder.AlterColumn<int>(
                name: "TrackId",
                table: "PlaylistItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Label",
                table: "PlaylistItems",
                type: "varchar(400)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistItems_Tracks_TrackId",
                table: "PlaylistItems",
                column: "TrackId",
                principalTable: "Tracks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistItems_Tracks_TrackId",
                table: "PlaylistItems");

            migrationBuilder.AlterColumn<int>(
                name: "TrackId",
                table: "PlaylistItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "PlaylistItems",
                keyColumn: "Label",
                keyValue: null,
                column: "Label",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Label",
                table: "PlaylistItems",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(400)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistItems_Tracks_TrackId",
                table: "PlaylistItems",
                column: "TrackId",
                principalTable: "Tracks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
