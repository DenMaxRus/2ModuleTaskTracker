using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace _2ModuleTaskTracker
{
    public class Task : INotifyPropertyChanged
    {
        private string _name;
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged(); } }
        private double _duration;
        public double Duration { get { return _duration; } set { _duration = value; OnPropertyChanged(); } }
        private Responsible _responsible;
        public Responsible Responsible
        {
            get => _responsible;
            set { if (value != null) _responsible = value; OnPropertyChanged(); }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public Task()
        {
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
