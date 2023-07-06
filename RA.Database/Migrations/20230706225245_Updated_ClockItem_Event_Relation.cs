using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RA.Database.Migrations
{
    /// <inheritdoc />
    public partial class Updated_ClockItem_Event_Relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClockItems_ClockItems_ClockItemEventId",
                table: "ClockItems");

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

            migrationBuilder.InsertData(
                table: "UserGroupRules",
                columns: new[] { "Id", "RuleValue", "UserGroupId" },
                values: new object[] { 5, 4, 1 });

            migrationBuilder.UpdateData(
                table: "UserGroups",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsBuiltIn", "Name" },
                values: new object[] { true, "Administrators" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FullName", "Password", "Username" },
                values: new object[] { "Administrator", "$2a$11$XLzkiZw03i/cqn90F8cgr.EjrnXds.O7quStlYO7RI0H3BFxwg59e", "admin" });

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
                name: "FK_ClockItems_ClockItems_ClockItemEventId",
                table: "ClockItems",
                column: "ClockItemEventId",
                principalTable: "ClockItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClockItems_ClockItems_ClockItemTrackId",
                table: "ClockItems",
                column: "ClockItemTrackId",
                principalTable: "ClockItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClockItems_ClockItems_ClockItemCategoryId",
                table: "ClockItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ClockItems_ClockItems_ClockItemEventId",
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

            migrationBuilder.DeleteData(
                table: "UserGroupRules",
                keyColumn: "Id",
                keyValue: 5);

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

            migrationBuilder.UpdateData(
                table: "UserGroups",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsBuiltIn", "Name" },
                values: new object[] { false, "Administrator" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FullName", "Password", "Username" },
                values: new object[] { "Andrei", "$2a$11$SWq88W6Q77w7sanz7HrxbexnTN0nLq8XB70lLFrSDQbddPzmnQdIK", "andrei" });

            migrationBuilder.AddForeignKey(
                name: "FK_ClockItems_ClockItems_ClockItemEventId",
                table: "ClockItems",
                column: "ClockItemEventId",
                principalTable: "ClockItems",
                principalColumn: "Id");
        }
    }
}
