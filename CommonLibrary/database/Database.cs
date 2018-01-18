using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json; 

namespace CommonLibrary.database {
	public class Database<T> {

		private String FilePath { get; set; }
		private List<T> Objects { get; } = new List<T>();

		public static Database<T> Create(string filePath) {
			Database<T> dataBase = new Database<T> {
				FilePath = filePath,
			};

			dataBase.Read(filePath);

            return dataBase;
		}

        private Database()
        {
			Objects = new List<T>();
        }

		public void Write() {
			Write(FilePath);
		}

		public void Write(string filePath) {
            File.WriteAllText(filePath, JsonConvert.SerializeObject(Objects));
		}

		public void Read(string filePath) {
			if(filePath != null && File.Exists(filePath)) {
				FilePath = filePath;
				string content = File.ReadAllText(filePath);
				Objects.Clear();
				Objects.AddRange(JsonConvert.DeserializeObject<IEnumerable<T>>(content));
			}
		}

		public void Add(T entry) { Objects.Add(entry); }
		public void Remove(T entry) { Objects.Remove(entry); }
		public List<T> Select() { return Objects; }
	}
}
