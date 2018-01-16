using System;
using System.Security.Cryptography;
using System.Text;

public enum UserAccessLevel
{
    Default,
    Admin
}

public class User
{
    private string passwordHash;

    public int ID { get; private set; }
    public string Login { get; private set; }
    public UserAccessLevel AccessLevel { get; private set; }

    private string RawToHash(string raw)
    {
        HashAlgorithm md5 = MD5.Create();

        StringBuilder sb = new StringBuilder();
        foreach (byte b in md5.ComputeHash(Encoding.UTF8.GetBytes(raw)))
            sb.Append(b.ToString("X2"));

        return sb.ToString();
    }

    public string Password
    {
        set { passwordHash = RawToHash(value); }
    }

    public bool IsCorrectPassword(string rawPassword)
    {
        return RawToHash(rawPassword).Equals(passwordHash);
    }

    public User(int id, string login, string rawPassword, UserAccessLevel level)
    {
        ID = id;
        Login = login;
        Password = rawPassword;
        AccessLevel = level;
    }
}