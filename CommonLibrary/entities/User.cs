using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CommonLibrary.entities {
	public class User : INotifyPropertyChanged {
		[JsonProperty]
		private string password;
		private int id;
		private string login;
		private UserAccessLevel accessLevel;
		public event PropertyChangedEventHandler PropertyChanged;

		private string RawToHash(string raw) {
			return Encoding.UTF8.GetString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(raw)));
		}

		public int Id {
			get => id;
			set {
				id = value;
				OnPropertyChanged();
			}
		}
		public string Login {
			get => login;
			set {
				login = value;
				OnPropertyChanged();
			}
		}

		[JsonIgnore]
		public string Password {
			get => password;
			set {
				password = RawToHash(value);
				OnPropertyChanged();
			}
		}

		[JsonConverter(typeof(StringEnumConverter))]
		public UserAccessLevel AccessLevel {
			get => accessLevel;
			set {
				accessLevel = value;
				OnPropertyChanged();
			}
		}

		public bool IsCorrectPassword(string rawPassword) {
			return RawToHash(rawPassword).Equals(password);
		}

		private User() { }

		public User(int id, string login, string rawPassword, UserAccessLevel level) {
			Id = id;
			Login = login;
			Password = rawPassword;
			AccessLevel = level;
		}

		public User(int id, string login, string rawPassword) : this(id, login, rawPassword, UserAccessLevel.Default) {
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