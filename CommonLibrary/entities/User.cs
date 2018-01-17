using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CommonLibrary.database {
	public class User : INotifyPropertyChanged {
		[JsonProperty]
		[JsonConverter(typeof(PasswordConverter))]
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

		public enum UserAccessLevel {
			Default,
			Admin
		}

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public class PasswordConverter : JsonConverter {
			public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
				writer.WriteValue(value as String);
			}

			public override bool CanRead {
				get { return true; }
			}

			public override bool CanConvert(Type objectType) {
				return objectType == typeof(String);
			}

			public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
				return reader.Value as String;
			}
		}
	}
}