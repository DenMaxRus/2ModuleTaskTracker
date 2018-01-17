﻿using CommonLibrary.database;
using System;
using System.Linq;

namespace CommonLibrary.entities {
	public class Authentication
    {
		private static readonly Lazy<Authentication> instance = new Lazy<Authentication>();

		public static Authentication Instance => instance.Value;

		public User CurrentUser { get; private set; }

        public bool Login(string login, string rawPassword, Database<User> database)
        {
            CurrentUser = null;

            if (database.Select().Count() == 0)
            {
				database.Add(new User(0, login, rawPassword, User.UserAccessLevel.Admin));
            }

            CurrentUser = database.Select().FirstOrDefault(
                u => login.Equals(u.Login) && u.IsCorrectPassword(rawPassword));
            
            return CurrentUser != null;
        }
    }
}