using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RA.Database.Migrations
{
    /// <inheritdoc />
    public partial class View_CategoryHierarchy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE VIEW CategoriesHierarchy AS 
                                  WITH RECURSIVE CategoryHierarchy AS
                                  (
                                  SELECT Id, NAME, ParentId, CAST(Name AS CHAR(500)) AS PathName, 0 AS LEVEL
                                  FROM Categories
                                  WHERE ParentId IS NULL 
                                  UNION ALL
                                  SELECT c.Id, c.NAME, c.ParentId, CONCAT(ch.PathName, ' > ', c.Name), ch.Level + 1
                                  FROM Categories c
                                  INNER JOIN CategoryHierarchy ch 
                                  ON c.ParentId = ch.Id
                                  )
                                  
                                  
                                  SELECT Id, Name, ParentId, Level, PathName
                                  FROM CategoryHierarchy ch;
                                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW CategoriesHierarchy;");
        }
    }
}
