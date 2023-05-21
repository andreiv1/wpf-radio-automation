using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using RA.Database;
using RA.Logic.Database;
using RA.UI.Core.Services;
using RA.UI.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace RA.UI.Core.Shared
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
        private readonly IMessageBoxService messageBoxService;

        public DatabaseSetupViewModel(IMessageBoxService messageBoxService)
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

            this.messageBoxService = messageBoxService;
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
        }

        [RelayCommand]
        private void TestConnection()
        {
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
                string connString = DatabaseCredentials.RetrieveConnectionString();

                optionsBuilder.UseMySql(connString, ServerVersion.AutoDetect(connString))
                    .EnableSensitiveDataLogging(false);

                using var dbContext = new AppDbContext(optionsBuilder.Options);
                var result = dbContext.Database.ExecuteSqlRaw("SELECT 1 FROM DUAL;");
                messageBoxService.ShowInfo($"Connected succesfully!");
            }
            catch (MySqlException e)
            {
                messageBoxService.ShowError($"Mysql error: {e.Message}");
            }
        }

        [RelayCommand]
        private void InstallDatabase()
        {
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
                string connString = DatabaseCredentials.RetrieveConnectionString();

                optionsBuilder.UseMySql(connString, ServerVersion.AutoDetect(connString))
                    .EnableSensitiveDataLogging(false);

                using var dbContext = new AppDbContext(optionsBuilder.Options);
                dbContext.Database.EnsureCreated();
                messageBoxService.ShowInfo($"Database created succesfully.");
            }
            catch (MySqlException e)
            {
                messageBoxService.ShowError($"Mysql error: {e.Message}");
            }
        }

        [RelayCommand]
        private void UpgradeDatabase()
        {
            messageBoxService.ShowWarning($"To do.");
        }
    }
}
