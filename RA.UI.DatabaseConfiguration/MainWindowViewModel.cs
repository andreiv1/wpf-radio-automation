using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using RA.Database;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RA.UI.DatabaseConfiguration
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private DatabaseCredentials databaseCredentials;
        public DatabaseCredentials DatabaseCredentials { get { return databaseCredentials; } }

        public String NewPassword { get; set; }

        public MainWindowViewModel()
        {
            databaseCredentials = new DatabaseCredentials();
            databaseCredentials.LoadCredentials();
        }

        private bool connectedToDatabase = false;

        [RelayCommand]
        private void SaveCredentials()
        {
            try
            {
                databaseCredentials.DatabasePassword = NewPassword;
                //using (var db = new AppDbContext())
                //{
                //    connectedToDatabase = db.Database.CanConnect();
                //    if(connectedToDatabase)
                //    {
                //        MessageBox.Show("Connected succesfully to database!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                //    } else
                //    {
                //        MessageBox.Show("Connection failed! Please check your credentials and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                //    }
                //    databaseCredentials.SaveCredentials();
                //    InstallDatabaseCommand.NotifyCanExecuteChanged();
                //    UpgradeDatabaseCommand.NotifyCanExecuteChanged();
                //}
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        [RelayCommand(CanExecute = nameof(IsConnectedToDatabase))]
        private void InstallDatabase()
        {
            //using(var db = new AppDbContext())
            //{
            //    db.Database.EnsureCreated();
            //}

        }

        [RelayCommand(CanExecute = nameof(IsConnectedToDatabase))]
        private void UpgradeDatabase()
        {
            //using(var db = new AppDbContext())
            //{
            //    db.Database.Migrate();
            //}
        }

        private bool IsConnectedToDatabase()
        {
            return connectedToDatabase;
        }
    }
}
