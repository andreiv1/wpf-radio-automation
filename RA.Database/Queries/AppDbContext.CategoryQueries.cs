using Microsoft.EntityFrameworkCore;
using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Database
{
    public partial class AppDbContext : DbContext
    {
        /// <summary>
        /// Get only the root categories (parents) without children
        /// </summary>
        /// <returns></returns>
        public IQueryable<Category> GetRootCategories()
        {
            return Categories.Where(c => c.ParentId == null);
        }

        public IQueryable<Category> GetChildrenCategories(int parentCategoryId)
        {
            return Categories.Where(c => c.ParentId == parentCategoryId);
        }

        public IQueryable<CategoryHierarchy> GetCategoriesHierarchyWithoutParents()
        {
            return CategoriesHierarchy.FromSqlRaw(@"SELECT Id, Name, ParentId, PathName, Level
                                                    FROM CategoriesHierarchy ch
                                                    WHERE NOT EXISTS (
                                                        SELECT 1
                                                        FROM Categories c
                                                        WHERE c.ParentId = ch.Id
                                                    );");
        }

        public TimeSpan GetCategoryAvgDuration(int categoryId)
        {
            double averageDurationInSeconds = 0;
            string sqlQuery = @$"SELECT AVG(Duration) FROM Categories_Tracks ct 
                                        JOIN Tracks t ON ct.TrackId = t.Id
                                        WHERE CategoryId={categoryId};";
            using (var command = Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sqlQuery;
                Database.OpenConnection();

                var result = command.ExecuteScalar();
                averageDurationInSeconds = result != DBNull.Value ? Convert.ToDouble(result) : 0;
                Database.CloseConnection();
            }
            return TimeSpan.FromSeconds(averageDurationInSeconds);
        }

    }
}
