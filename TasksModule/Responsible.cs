using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TasksModule
{
    public class Responsible: INotifyPropertyChanged
    {
        private int _id;
        public int Id { get { return _id; } set { _id = value; OnPropertyChanged(); } }
        private string _name;
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged(); } }
        public event PropertyChangedEventHandler PropertyChanged;

        public Responsible() { }

        public Responsible(int id, string name)
        {
            Name = name;
            Id = id;
        }

        public Responsible(string res)
        {
            if (res != null)
            {
                string[] parameters = res.Split(':');
                if (parameters.Length == 2)
                {
                    Id = Convert.ToInt32(parameters[0]);
                    Name = parameters[1];
                }
                else
                    throw new FormatException("Wrong string format for creating Responsible");
            }
        }

        public override string ToString()
        {
            return Id + ":" + Name;
        }

        public override bool Equals(object obj)
        {
            return obj is Responsible && Id == (obj as Responsible).Id;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override int GetHashCode()
        {
            return 1213502048 + Id.GetHashCode();
        }
        //public static bool operator !=(Responsible first, Responsible second) => first.ID != second.ID;
        //public static bool operator ==(Responsible first, Responsible second) => first.ID == second.ID;
    }
}
