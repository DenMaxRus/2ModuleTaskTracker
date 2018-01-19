using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CommonLibrary.entities {
	public class User : INotifyPropertyChanged, IEquatable<User> {
		[JsonProperty]
		private string password;
		private int id;
		private string login;
		private UserRole accessLevel;
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
		public UserRole AccessLevel {
			get => accessLevel;
			set {
				accessLevel = value;
				OnPropertyChanged();
			}
		}

		public bool IsCorrectPassword(string rawPassword) {
			return ConvertRawPassward(rawPassword).Equals(password);
		}

		private User(User user) {
			id = user.id;
			login = user.login;
			password = user.password;
			accessLevel = user.accessLevel;
		}

		[JsonConstructor]
		private User(int id, string login, string password, UserRole accessLevel) {
			this.id = id;
			this.login = login;
			this.password = password;
			this.accessLevel = accessLevel;
		}

		public User(string login, string rawPassword, UserRole level) {
			Id = -1;
			Login = login;
			Password = rawPassword;
			AccessLevel = level;
		}

		public User(string login, string rawPassword) : this(login, rawPassword, UserRole.User) {
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

	public enum UserRole {
		User,
		Admin
	}
}