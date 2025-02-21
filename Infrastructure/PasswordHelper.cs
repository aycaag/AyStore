using System.Security.Cryptography;
using System.Text;

public interface IPasswordHelper
{
    string HashPassword(string password);
}

public class PasswordHelper : IPasswordHelper
{
    public string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
