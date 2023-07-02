using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RA.Database.Migrations
{
    /// <inheritdoc />
    public partial class Modified_Playlist_TrackHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Playlists_Users_UserId",
                table: "Playlists");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackHistory_Tracks_TrackId",
                table: "TrackHistory");

            migrationBuilder.DropIndex(
                name: "IX_Playlists_UserId",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Playlists");

            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "PlaylistItems",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "ParentPlaylistItemId",
                table: "PlaylistItems",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Color",
                value: "#292928");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Color",
                value: "#f91212");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "Color",
                value: "#0ac720");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "Color",
                value: "#d016f5");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "Color",
                value: "#001dd9");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistItems_ParentPlaylistItemId",
                table: "PlaylistItems",
                column: "ParentPlaylistItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistItems_PlaylistItems_ParentPlaylistItemId",
                table: "PlaylistItems",
                column: "ParentPlaylistItemId",
                principalTable: "PlaylistItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackHistory_Tracks_TrackId",
                table: "TrackHistory",
                column: "TrackId",
                principalTable: "Tracks",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistItems_PlaylistItems_ParentPlaylistItemId",
                table: "PlaylistItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackHistory_Tracks_TrackId",
                table: "TrackHistory");

            migrationBuilder.DropIndex(
                name: "IX_PlaylistItems_ParentPlaylistItemId",
                table: "PlaylistItems");

            migrationBuilder.DropColumn(
                name: "Label",
                table: "PlaylistItems");

            migrationBuilder.DropColumn(
                name: "ParentPlaylistItemId",
                table: "PlaylistItems");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Playlists",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Color",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Color",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "Color",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "Color",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "Color",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_UserId",
                table: "Playlists",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Playlists_Users_UserId",
                table: "Playlists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackHistory_Tracks_TrackId",
                table: "TrackHistory",
                column: "TrackId",
                principalTable: "Tracks",
                principalColumn: "Id");
        }
    }
}
