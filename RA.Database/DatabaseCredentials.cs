using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Markup;
using System.Security.Cryptography;

namespace RA.Database
{
    public class DatabaseCredentials
    {
        public string? Host { get; set; } = "localhost";
        public string? Port { get; set; } = "3306";
        public string? DatabaseName { get; set; } = "RA";
        public string? DatabaseUser { get; set; } = "root";
        public string? DatabasePassword { get; set; }

        public DatabaseCredentials()
        {
            
        }

        public void SaveCredentials()
        {
            string jsonData = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var subfolderPath = Path.Combine(folderPath, "RAsoftware");
            Directory.CreateDirectory(subfolderPath);
            var filePath = Path.Combine(subfolderPath, "database.json");
            File.WriteAllText(filePath, jsonData);
        }

        public void LoadCredentials() {
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var subfolderPath = Path.Combine(folderPath, "RAsoftware");
            var filePath = Path.Combine(subfolderPath, "database.json");
            try
            {
                var jsonData = File.ReadAllText(filePath);
                var data = JsonSerializer.Deserialize<DatabaseCredentials>(jsonData);
                this.Host = data.Host;
                this.Port = data.Port;
                this.DatabaseName = data.DatabaseName;
                this.DatabaseUser = data.DatabaseUser;
                this.DatabasePassword = data.DatabasePassword;
            }
            catch(Exception e)
            {

            }
        }

        public override string ToString()
        {
            return $"server={Host};Port={Port};database={DatabaseName};user={DatabaseUser};password={DatabasePassword}";
        }
    }
}
