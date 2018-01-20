using System.Linq;
using CommonLibrary.entities;

namespace CommonLibrary.database {
	public class UserRoleDatabase : Database<UserRole> {
		private readonly static string UnknownId = null;
		private static int GlobalId { get; set; } = 1;

		public override bool Add(UserRole entry) {
			entry.Id = "role_" + GlobalId;
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

        public override Database<UserRole> Read (string filePath)
        {
            var result = base.Read(filePath);
			if(Select().Count != 0) {
				GlobalId = Select().Max(e => int.Parse(e.Id.Substring("role_".Length))) + 1;
			}
            return result;
        }
	}
}
