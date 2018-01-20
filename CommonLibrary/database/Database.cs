using System;
using System.Collections.Generic;
using System.IO;
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
			File.WriteAllText(filePath, JsonConvert.SerializeObject(Entries.Keys as IEnumerable<T>));
		}

        public virtual Database<T> Read(string filePath) {
			FilePath = filePath;
			Drop();
			if(FilePath != null && File.Exists(FilePath)) {
				foreach(var entry in JsonConvert.DeserializeObject<IEnumerable<T>>(File.ReadAllText(FilePath))) {
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
