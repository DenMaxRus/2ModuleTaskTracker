using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.database {
	public class Database<T> {
		public List<T> objects = new List<T>();

		public static Database<T> Read(string filePath) {
			return new Database<T>();
		}

		public void Write(string filePath) {
		}

		public void Add(T entry) {
			objects.Add(entry);
		}
		public void Remove(T entry) { }
		public void Update(T entry) { }
		public IEnumerable<T> Select() {
			return objects;
		}
	}
}
