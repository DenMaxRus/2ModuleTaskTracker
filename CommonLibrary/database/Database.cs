using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace CommonLibrary.database {
	public class Database<T> {

		private String FilePath { get; set; }
		private Dictionary<T, bool> Entries { get; set; } = new Dictionary<T, bool>();

		public Database() {
		}

		public void Write() {
			Write(FilePath);
		}

		public void Write(string filePath) {
			var encodedJson = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Entries.Keys as IEnumerable<T>)));
			File.WriteAllText(filePath, encodedJson);
		}

        public virtual Database<T> Read(string filePath) {
			FilePath = filePath;
			Drop();
			if(FilePath != null && File.Exists(FilePath)) {
				var decodedJson = Encoding.UTF8.GetString(Convert.FromBase64String(File.ReadAllText(FilePath)));
				foreach(var entry in JsonConvert.DeserializeObject<IEnumerable<T>>(decodedJson)) {
                    InternalAddFromJson(entry);
				}
			}

			return this;
		}

        private void InternalAddFromJson(T entry)
        {
            Entries.Add(entry, false);
        }

		public virtual bool Add(T entry) {
			var isNewEntry = !Entries.ContainsKey(entry);
			Entries.Add(entry, false);

			return isNewEntry;
		}
		public virtual bool Update(T entry) {
			if(Entries.Remove(entry)) {
				Entries.Add(entry, false);
				return true;
			}

			return false;
		}
		public virtual void AddOrUpdate(T entry) {
			if(!Update(entry)) {
				Add(entry);
			}
		}
		public virtual bool Remove(T entry) { return Entries.Remove(entry); }
		public virtual IReadOnlyCollection<T> Select() { return Entries.Keys; }
		public virtual void Drop() { Entries.Clear(); }
	}
}
