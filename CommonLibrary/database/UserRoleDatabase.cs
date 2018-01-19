using System.Linq;
using CommonLibrary.entities;

namespace CommonLibrary.database {
	public class UserRoleDatabase : Database<UserRole> {
		private static int GlobalId { get; set; } = 1;

		public override bool Add(UserRole entry) {
			if(base.Add(entry)) {
				entry.Id = "role_" + GlobalId++;
				return true;
			}

			return false;
		}

		public override void Drop() {
			base.Drop();
			GlobalId = 1;
		}

        public override Database<UserRole> Read (string filePath)
        {
            var result = base.Read(filePath);
            GlobalId = Select().Max(e => int.Parse(e.Id.Substring ("role_".Length))) + 1;
            return result;
        }
	}
}
