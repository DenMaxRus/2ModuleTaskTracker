using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CommonLibrary.entities {
	public class Employer : INotifyPropertyChanged {
		private static int GlobalId { get; set; } = 0;

		private int id;
		private string occupation;
		private string name;
		private double salary;
		public event PropertyChangedEventHandler PropertyChanged;

		public int Id {
			get => id;
			set {
				id = value;
				OnPropertyChanged();
			}
		}
		public string Occupation {
			get => occupation;
			set {
				occupation = value;
				OnPropertyChanged();
			}
		}
		public string Name {
			get => name;
			set {
				name = value;
				OnPropertyChanged();
			}
		}
		public double Salary {
			get => salary;
			set {
				salary = value;
				OnPropertyChanged();
			}
		}

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
