using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CommonLibrary.entities {
    public class Employee : INotifyPropertyChanged, System.IEquatable<Employee> {
        private static int GlobalId { get; set; } = 0;

        private int id;
        private string occupation;
        private string name;
        private double salary;
        private int hoursPerDay;
        private bool hasWorkOpportunity;
        public event PropertyChangedEventHandler PropertyChanged;

        public int Id {
            get => id;
            private set {
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
                if (value > 0) {
                    salary = value;
                    OnPropertyChanged();
                }
            }
        }
        public int HoursPerDay {
            get => hoursPerDay;
            set {
                if ((value > 0) && (value < 24)) {
                    hoursPerDay = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool HasWorkOpportunity {
            get => hasWorkOpportunity;
            set {
                hasWorkOpportunity = value;
                OnPropertyChanged();
            }
        }

        public Employee () : this(typeof(Employee).Name + "_" + GlobalId, "unknown", 0, 0, true) {
        }

        public Employee (string name, string occupation, double salary, int hoursPerDay, bool hasWorkOpportunity) {
            Id = GlobalId++;
            Occupation = occupation;
            Name = name;
            Salary = salary;
            HoursPerDay = hoursPerDay;
            HasWorkOpportunity = hasWorkOpportunity;
        }

        protected void OnPropertyChanged ([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }        

        public bool Equals (Employee other) {
            return other != null && id.Equals(other.id);
        }

        public override bool Equals (object obj) {
            return Equals(obj as Employee);
        }

        public override int GetHashCode () {
            return id.GetHashCode();
        }
    }
}
