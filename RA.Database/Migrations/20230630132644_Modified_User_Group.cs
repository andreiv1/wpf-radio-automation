using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RA.Database.Migrations
{
    /// <inheritdoc />
    public partial class Modified_User_Group : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserGroupUserGroupRule");

            migrationBuilder.AddColumn<int>(
                name: "UserGroupId",
                table: "UserRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserRules_UserGroupId",
                table: "UserRules",
                column: "UserGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRules_UserGroups_UserGroupId",
                table: "UserRules",
                column: "UserGroupId",
                principalTable: "UserGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRules_UserGroups_UserGroupId",
                table: "UserRules");

            migrationBuilder.DropIndex(
                name: "IX_UserRules_UserGroupId",
                table: "UserRules");

            migrationBuilder.DropColumn(
                name: "UserGroupId",
                table: "UserRules");

            migrationBuilder.CreateTable(
                name: "UserGroupUserGroupRule",
                columns: table => new
                {
                    GroupsId = table.Column<int>(type: "int", nullable: false),
                    RulesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroupUserGroupRule", x => new { x.GroupsId, x.RulesId });
                    table.ForeignKey(
                        name: "FK_UserGroupUserGroupRule_UserGroups_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "UserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroupUserGroupRule_UserRules_RulesId",
                        column: x => x.RulesId,
                        principalTable: "UserRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupUserGroupRule_RulesId",
                table: "UserGroupUserGroupRule",
                column: "RulesId");
        }
    }
}
