using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json; 

namespace CommonLibrary.database {
	public class Database<T> {

        public List<T> data;

		public static Database<T> Read(string filePath) {
            Database<T> dataBase = new Database<T>();

            if (File.Exists(filePath))
            {
                string content = File.ReadAllText(filePath);
                dataBase.data = JsonConvert.DeserializeObject<List<T>>(content);
            }

            return dataBase;
		}

        private Database()
        {
            data = new List<T>();
        }

		public void Write(string filePath) {
            File.WriteAllText(filePath, JsonConvert.SerializeObject(data));
		}

		public void Add(T entry) { data.Add(entry); }
		public void Remove(T entry) { data.Remove(entry); }
		public List<T> Select() { return data; }
	}
}
