using System;
using System.Security.Cryptography;
using System.Text;

public static class HashHelper
{
    public static string GetHash(string input)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    public static string HashPassword(string password)
    {
        return GetHash(password);
    }

    public static string HashSecretWord(string secretWord)
    {
        return GetHash(secretWord);
    }

    public static bool VerifyHash(string input, string hash)
    {
        string hashOfInput = GetHash(input);
        StringComparer comparer = StringComparer.OrdinalIgnoreCase;
        return comparer.Compare(hashOfInput, hash) == 0;
    }
}
