using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RA.DAL;
using RA.Database;

namespace RA.ConsoleApp
{
    public class DbContextFactory : IDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext()
        {
            DbContextOptionsBuilder<AppDbContext> optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            String connString = "server=192.168.200.113;Port=3306;database=rasoftware;user=root;password=andrewyw1412";
            DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptions<AppDbContext>();

            dbContextOptions = optionsBuilder.UseMySql(connString, ServerVersion.AutoDetect(connString))
                .EnableSensitiveDataLogging(false)
                .Options;
            return new AppDbContext(dbContextOptions);
        }
    }
    public class Program
    {
        static void Main(string[] args)
        {
            var dbFactory = new DbContextFactory();
            ICategoriesService categoriesService = new CategoriesService(dbFactory);

            Console.WriteLine("Root categories:");
            var rootCat = categoriesService.GetRootCategoriesAsync();
            foreach(var category in rootCat.Result)
            {
                Console.WriteLine($"{category.Name}, Id={category.Id}");
                if (categoriesService.HasCategoryChildren(category.Id!.Value).Result)
                {
                    ShowCategoriesRecursive(categoriesService, category.Id!.Value, " ");
                }
            }
        }

        static void ShowCategoriesRecursive(ICategoriesService categoriesService, int? parentId, string prefix)
        {
            var categories = categoriesService.GetChildrenCategoriesAsync(parentId!.Value).Result;
            foreach (var category in categories)
            {
                Console.WriteLine($"{prefix} {category.Name}, Id={category.Id}");
                ShowCategoriesRecursive(categoriesService, category.Id, prefix + "    ");
            }
        }
    }
}