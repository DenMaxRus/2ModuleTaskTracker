using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public Database<T> GetDatabase<T>() {
			return DatabaseStorage[typeof(T)] as Database<T>;
		}
	}
}
