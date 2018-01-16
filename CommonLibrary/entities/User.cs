using System;
using System.Security.Cryptography;
using System.Text;

namespace CommonLibrary.database {
	public class User {
        
        public string PasswordHash { get; set; }
		public int ID { get; set; }
		public string Login { get; set; }
		public UserAccessLevel AccessLevel { get; set; }

		private string RawToHash(string raw) {
			HashAlgorithm md5 = MD5.Create();

			StringBuilder sb = new StringBuilder();
			foreach(byte b in md5.ComputeHash(Encoding.UTF8.GetBytes(raw)))
				sb.Append(b.ToString("X2"));

			return sb.ToString();
		}

		public string Password {
			set { PasswordHash = RawToHash(value); }
		}

		public bool IsCorrectPassword(string rawPassword) {
			return RawToHash(rawPassword).Equals(PasswordHash);
		}

		public User(int id, string login, string rawPassword, UserAccessLevel level) {
			ID = id;
			Login = login;
			Password = rawPassword;
			AccessLevel = level;
		}

		public enum UserAccessLevel {
			Default,
			Admin
		}
	}
}