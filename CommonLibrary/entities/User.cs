using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using CommonLibrary.database;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CommonLibrary.entities {
    public class User : INotifyPropertyChanged
    {
        private static int GlobalId { get; set; } = 0;

        [JsonProperty]
        private string password;

        private string roleID;
        private int id;
        private string login;
        private UserAccessLevel accessLevel;
        public event PropertyChangedEventHandler PropertyChanged;

        public static string ConvertRawPassward(string rawPassword)
        {
            return Encoding.UTF8.GetString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(rawPassword)));
        }

        public string RoleId
        {
            get => roleID;
            set
            {
                roleID = value;
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

        // Не стал это удалять потому что не знаем заранее о возможных правах
        // но хотелось бы чтобы первый юзер автоматом обладал всем
        // можно заменить на bool IsGodUser но лень и нет смысла
        [JsonConverter(typeof(StringEnumConverter))]
        public UserAccessLevel AccessLevel
        {
            get => accessLevel;
            set
            {
                accessLevel = value;
                OnPropertyChanged();
            }
        }

        public bool IsCorrectPassword(string rawPassword)
        {
            return ConvertRawPassward(rawPassword).Equals(password);
        }

        public bool IsHaveAccessTo(string module, string action)
        {
            if (AccessLevel == UserAccessLevel.Admin)
                return true;

            UserRole findRole = DatabaseManager.Instance
                                               .GetDatabase<UserRole>()
                                               .Select()
                                               .FirstOrDefault(role => role.Id.Equals(RoleId));
            if (findRole == null)
                return false;

            return findRole.IsHaveAccessTo(module, action);
        }

        public User(string login, string rawPassword, UserAccessLevel level, string roleId)
        {
            Id = GlobalId++;
            Login = login;
            Password = rawPassword;
            AccessLevel = level;
            RoleId = roleId;
        }

        public User(string login, string rawPassword, UserAccessLevel level) : this(login, rawPassword, level, string.Empty) {}

		public User(string login, string rawPassword) : this(login, rawPassword, UserAccessLevel.Default, string.Empty) {
		}

		public User() : this("user_" + GlobalId, String.Empty) {
		}

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public enum UserAccessLevel {
		Default,
		Admin
	}
}