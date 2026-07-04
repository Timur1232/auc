using System.Security.Cryptography;
namespace App.Services;

public class PasswordHasher
{
    private const int SALT_SIZE = 16;
    private const int HASH_SIZE = 32;
    private const int ITERATIONS = 100000;

    private readonly HashAlgorithmName algorithm = HashAlgorithmName.SHA256;

    public string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SALT_SIZE);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, ITERATIONS, algorithm, HASH_SIZE);
        return $"{Convert.ToBase64String(hash)}-{Convert.ToBase64String(salt)}";
    }

    public bool Varify(string password, string password_hash)
    {
        string[] pair = password_hash.Split('-');
        byte[] hash = Convert.FromBase64String(pair[0]);
        byte[] salt = Convert.FromBase64String(pair[1]);
        byte[] input_hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, ITERATIONS, algorithm, HASH_SIZE);
        return CryptographicOperations.FixedTimeEquals(hash, input_hash);
    }
}
