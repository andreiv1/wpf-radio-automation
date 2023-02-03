namespace RA.DAL.Utils
{
    public class PasswordEncryptor
    {
        public static string EncryptPassword(string password)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
            return hashedPassword;
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            return isValidPassword;
        }
    }
}
