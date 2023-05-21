using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RA.Database;
using RA.Logic;
using RA.Logic.Database;
using RA.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace RA.UI.StationManagement
{
    public partial class DatabaseSetupViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string server = "localhost";
        [ObservableProperty]
        private string dbName = "RA";
        [ObservableProperty]
        private string dbUser = "root";
        [ObservableProperty]
        private string dbPassword = "";

        public DatabaseSetupViewModel()
        {
            var creds = DatabaseCredentials.GetCredentials();
            if (creds.Count > 0)
            {
                Server = creds["server"];
                if (creds.ContainsKey("port"))
                {
                    if (creds["port"] != "3306")
                    {
                        server += $":{creds["port"]}";
                    }
                }
                DbName = creds["database"];
                DbUser = creds["user"];
                DbPassword = creds["password"];
            }
        }

        [RelayCommand]
        private void SaveCredentials()
        {
            Dictionary<string, string> creds = new();
            if (Server.Contains(":"))
            {
                var tokens = Server.Split(':');
                creds.Add("server", tokens[0]);
                creds.Add("port", tokens[1]);
            }
            else
            {
                creds.Add("server", Server);
            }
            creds.Add("database", DbName);
            creds.Add("user", DbUser);
            creds.Add("password", DbPassword); 
            DatabaseCredentials.StoreCreds(creds);

            //var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            //    .Options.Use;
            
            //var dbContext = new AppDbContext(dbContextOptions);
            //var result = dbContext.Database.ExecuteSqlRaw("SELECT 1 FROM DUAL;");

        }

        [RelayCommand]
        private void InstallDatabase()
        {

        }

        [RelayCommand]
        private void UpgradeDatabase()
        {

        }
    }
}
