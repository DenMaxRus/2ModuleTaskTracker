﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using CommonLibrary.database;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CommonLibrary.entities {
	public class User : INotifyPropertyChanged, IEquatable<User> {
		[JsonProperty]
		private string password;
		private int id;
		private string login;
		private string roleId;
		public event PropertyChangedEventHandler PropertyChanged;

        public static string ConvertRawPassward(string rawPassword)
        {
            return Encoding.UTF8.GetString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(rawPassword)));
        }

        public string RoleId
        {
            get => roleId;
            set
            {
                roleId = value;
                OnPropertyChanged();
            }
        }

        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }
        public string Login
        {
            get => login;
            set
            {
                login = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public string Password
        {
            get => password;
            set
            {
                password = ConvertRawPassward(value);
                OnPropertyChanged();
            }
        }

        public bool IsCorrectPassword(string rawPassword)
        {
            return ConvertRawPassward(rawPassword).Equals(password);
        }

		private User(User user) : this(user.login, user.password, user.roleId) {
			id = user.id;
		}

		[JsonConstructor]
		private User(int id, string login, string password, string roleId) {
			this.id = id;
			this.login = login;
			this.password = password;
			this.roleId = roleId;
		}

		public User(string login, string rawPassword, string roleId) {
			Id = -1;
			Login = login;
			Password = rawPassword;
			RoleId = roleId;
		}
        public bool IsHaveAccessTo(string module, string action)
        {
            var userRole = DatabaseManager.Instance
                                               .GetDatabase<UserRole>()
                                               .Select()
                                               .FirstOrDefault(role => role.Id.Equals(RoleId));

            if (userRole == null)
                return false;

            if (userRole.IsHaveAccessTo("ALL", "ALL"))
                return true;

            return userRole.IsHaveAccessTo(module, action);
        }

		public User(string login, string rawPassword) : this(login, rawPassword, null) {
		}

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public static User Copy(User copyFrom) {
			return new User(copyFrom);
		}

		public bool Equals(User other) {
			return other != null && id.Equals(other.id);
		}

		public override bool Equals(object obj) {
			return Equals(obj as User);
		}

		public override int GetHashCode() {
			return id.GetHashCode();
		}
	}
}