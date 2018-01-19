using System;
using System.Collections.Generic;

namespace CommonLibrary.database {
	public class DatabaseManager {
		private static readonly Lazy<DatabaseManager> instance = new Lazy<DatabaseManager>();

		public static DatabaseManager Instance => instance.Value;

		private Dictionary<Type, object> DatabaseStorage { get; } = new Dictionary<Type, object>();

		public void RegisterDatabase<T>(Database<T> database) {
			if(!DatabaseStorage.ContainsKey(typeof(T))) {
				DatabaseStorage.Add(typeof(T), database);
			}
		}

		public void UnregisterDatabase<T>() {
			DatabaseStorage.Remove(typeof(T));
		}

		public Database<T> GetDatabase<T>() {
			if(DatabaseStorage.ContainsKey(typeof(T))) {
				return DatabaseStorage[typeof(T)] as Database<T>;
			}

			return null;
		}

		public void Close() {
			foreach(var keyValue in DatabaseStorage) {
				(keyValue.Value as dynamic).Write();
			}
			DatabaseStorage.Clear();
		}
	}
}
