using System.Linq;
using CommonLibrary.entities;

namespace CommonLibrary.database {
	public class UserDatabase : Database<User> {
		private readonly static int UnknownId = -1;
		private static int GlobalId { get; set; } = 1;

		public override bool Add(User entry) {
			entry.Id = GlobalId;
			if(base.Add(entry)) {
				GlobalId++;
				return true;
			}

			entry.Id = UnknownId;
			return false;
		}

		public override void Drop() {
			base.Drop();
			GlobalId = 1;
		}

        public override Database<User> Read (string filePath)
        {
            var result = base.Read (filePath);
			if(Select().Count != 0) {
				GlobalId = Select().Max(e => e.Id) + 1;
			}
            return result;
        }
	}
}
