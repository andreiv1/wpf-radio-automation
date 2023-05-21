using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CredentialManagement;

namespace RA.Logic.Database
{
    public class DatabaseCredentials
    {
        private static String target = "RadioAutomationSystem";
        private static void StoreConnectionString(string connectionString)
        {
            using(var cred = new Credential())
            {
                cred.Target = target;
                cred.Password = connectionString;
                cred.Type = CredentialType.Generic;

                cred.Save();
            }
        }

        public static void StoreCreds(Dictionary<string, string> credentials)
        {
            StoreConnectionString(MakeConnectionString(credentials));
        }

        public static string MakeConnectionString(Dictionary<string, string> credentials)
        {
            string port = credentials.ContainsKey("port") ? credentials["port"] : "3306";
            string server = credentials["server"];
            string database = credentials["database"];
            string user = credentials["user"];
            string password = credentials.ContainsKey("password") ? credentials["password"] : "";
            return $"server={server};Port={port};database={database};user={user};password={password}";
        }

        public static string RetrieveConnectionString()
        {
            using (var cred = new Credential())
            {
                cred.Target = target;

                if (cred.Load())
                {
                    return cred.Password;
                }
            }

            return String.Empty;
        }

        public static Dictionary<string, string> GetCredentials()
        {
            var result = new Dictionary<string, string>();
            string connString = RetrieveConnectionString();
            if (!String.IsNullOrEmpty(connString))
            {
                var creds = connString.Split(";");
                foreach (var cred in creds)
                {
                    var tokens = cred.Split("=");
                    result.Add(tokens[0].ToLower(), tokens[1]);
                }
            }
            return result;
        }
    }
}
