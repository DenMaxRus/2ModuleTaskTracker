using CommonLibrary.database;
using System.Linq;

namespace CommonLibrary.entities
{
    public class Authentication
    {
        private Database<User> database;

        public User CurrentUser { get; private set; }

        public Authentication(Database<User> database)
        {
            this.database = database;
        }

        public bool Login(string login, string rawPassword)
        {
            CurrentUser = null;

            if (!database.Select().Any())
            {
                User admin = new User(0, login, rawPassword, User.UserAccessLevel.Admin);
                database.Add(admin);
            }

            CurrentUser = database.Select().FirstOrDefault(
                u => login.Equals(u.Login) && u.IsCorrectPassword(rawPassword));
            
            return CurrentUser != null;
        }
    }
}
