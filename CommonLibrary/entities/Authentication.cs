using CommonLibrary.database;
using System;
using System.Linq;

namespace CommonLibrary.entities {
	public class Authentication
    {
		private static readonly Lazy<Authentication> instance = new Lazy<Authentication>();

		public static Authentication Instance => instance.Value;

		public User CurrentUser { get; private set; }

        public bool Login(string login, string rawPassword)
        {
			var userDatabase = DatabaseManager.Instance.GetDatabase<User>();

			CurrentUser = null;

            if (userDatabase.Select().Count() == 0)
            {
				var rolesDatabase = DatabaseManager.Instance.GetDatabase<UserRole>();
				var godRole = rolesDatabase.Select().First();
				userDatabase.Add(new User(login, rawPassword) {
					RoleId = godRole.Id
				});
			}

            CurrentUser = userDatabase.Select().FirstOrDefault(
                u => login.Equals(u.Login) && u.IsCorrectPassword(rawPassword));
            
            return CurrentUser != null;
        }

		public void Logout() {
			CurrentUser = null;
		}
    }
}
