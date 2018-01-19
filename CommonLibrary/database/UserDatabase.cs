using System.Collections.Generic;
using System.Linq;
using CommonLibrary.database;
using CommonLibrary.entities;

namespace CommonLibrary.database {
	public class UserDatabase : Database<User> {
		private static int GlobalId { get; set; } = 1;

		public override bool Add(User entry) {
			if(base.Add(entry)) {
				entry.Id = GlobalId++;
				return true;
			}

			return false;
		}

		public override void Drop() {
			base.Drop();
			GlobalId = 1;
		}
	}
}
