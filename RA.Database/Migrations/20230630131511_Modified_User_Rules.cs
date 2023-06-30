using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RA.Database.Migrations
{
    /// <inheritdoc />
    public partial class Modified_User_Rules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserGroups_UserRules");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserGroupUserGroupRule");

            migrationBuilder.CreateTable(
                name: "UserGroups_UserRules",
                columns: table => new
                {
                    UserGroupId = table.Column<int>(type: "int", nullable: false),
                    UserRuleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups_UserRules", x => new { x.UserGroupId, x.UserRuleId });
                    table.ForeignKey(
                        name: "FK_UserGroupsUserRules_UserGroupId",
                        column: x => x.UserGroupId,
                        principalTable: "UserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroupsUserRules_UserRuleId",
                        column: x => x.UserRuleId,
                        principalTable: "UserRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_UserRules_UserRuleId",
                table: "UserGroups_UserRules",
                column: "UserRuleId");
        }
    }
}
