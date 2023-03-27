using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.UI.Core.Shared
{
    public class DatabaseConnectionException : Exception
    {
        public DatabaseConnectionException(string? message) : base(message)
        {
        }

        public DatabaseConnectionException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
    public static class DatabaseConnectionTester
    {
        public static async Task<bool> CanConnectToDatabase(DbContext dbContext)
        {
            try
            {
                var result = await dbContext.Database.ExecuteSqlRawAsync("SELECT 1 FROM Tracks;");
                return true;
            }
            catch (MySqlException ex)
            {
                throw new DatabaseConnectionException($"Failed to connect to the database: {ex.Message}", ex);
            }
            catch(Exception ex)
            {
                throw new DatabaseConnectionException($"Unknown error: {ex.Message}", ex);
            }
        }
    }
}
