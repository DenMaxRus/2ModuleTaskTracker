using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CommonLibrary.entities {
	public class User : INotifyPropertyChanged {
		private static int GlobalId { get; set; } = 0;

		[JsonProperty]
		private string password;
		private int id;
		private string login;
		private UserAccessLevel accessLevel;
		public event PropertyChangedEventHandler PropertyChanged;

		public static string ConvertRawPassward(string rawPassword) {
			return Encoding.UTF8.GetString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(rawPassword)));
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
				password = ConvertRawPassward(value);
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
			return ConvertRawPassward(rawPassword).Equals(password);
		}

		public User(string login, string rawPassword, UserAccessLevel level) {
			Id = GlobalId++;
			Login = login;
			Password = rawPassword;
			AccessLevel = level;
		}

		public User(string login, string rawPassword) : this(login, rawPassword, UserAccessLevel.Default) {
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