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
        return raw; // TODO HASH
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