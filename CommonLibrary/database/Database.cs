using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json; 

namespace CommonLibrary.database {
	public class Database<T> {

		private String FilePath { get; set; }
		private List<T> Objects { get; } = new List<T>();

		public static Database<T> Read(string filePath) {
			Database<T> dataBase = new Database<T> {
				FilePath = filePath,
			};

            if (File.Exists(filePath))
            {
                string content = File.ReadAllText(filePath);
				dataBase.Objects.Clear();
				dataBase.Objects.AddRange(JsonConvert.DeserializeObject<List<T>>(content));
            }

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

		public void Add(T entry) { Objects.Add(entry); }
		public void Remove(T entry) { Objects.Remove(entry); }
		public List<T> Select() { return Objects; }
	}
}
