using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.entities {
	public class Employer : INotifyPropertyChanged {
		private static int GlobalId { get; set; } = 0;

		private int id;
		private string occupation;
		private string name;
		private double salary;
		public event PropertyChangedEventHandler PropertyChanged;

		public int Id { get => id; set => id = value; }
		public string Occupation { get => occupation; set => occupation = value; }
		public string Name { get => name; set => name = value; }
		public double Salary { get => salary; set => salary = value; }

		public Employer() : this("employer_" + GlobalId, "unknown", 0) {
		}

		public Employer(string name, string occupation, double salary) {
			Id = GlobalId++;
			Occupation = occupation;
			Name = name;
			Salary = salary;
		}

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
