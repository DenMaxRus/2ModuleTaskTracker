using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommonLibrary.entities;
using Newtonsoft.Json;

namespace TasksModule
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
        private int _completePercentage;
        public int CompletePercentage
        {
            get { return _completePercentage; }
            set
            {
                _completePercentage = value;
                if (_completePercentage == 0)
                    Status = TaskStatus.NotStarted;
                else if (_completePercentage == 100)
                    Status = TaskStatus.Completed;
                else
                    Status = TaskStatus.InWork;
                OnPropertyChanged();
            }
        }
        private TaskStatus _status;
        public TaskStatus Status { get { return _status; } private set { _status = value; OnPropertyChanged(); } }
        private string _description;
        public string Description { get { return _description; } set { _description = value; OnPropertyChanged(); } }
        private User _author;
        [JsonProperty]
        public User Author { get { return _author; } private set { _author = value; OnPropertyChanged(); } }
        private DateTime _creationDate;
        [JsonProperty(ItemConverterType = typeof(Newtonsoft.Json.Converters.JavaScriptDateTimeConverter))]
        public DateTime CreationDate { get { return _creationDate; } private set { _creationDate = value; OnPropertyChanged(); } }
        private DateTime _startDate;
        [JsonProperty(ItemConverterType = typeof(Newtonsoft.Json.Converters.JavaScriptDateTimeConverter))]
        public DateTime StartDate { get { return _startDate; } set { _startDate = value; OnPropertyChanged(); } }
        private DateTime _endDate;
        [JsonProperty(ItemConverterType = typeof(Newtonsoft.Json.Converters.JavaScriptDateTimeConverter))]
        public DateTime EndDate { get { return _endDate; } set { _endDate = value; OnPropertyChanged(); } }

        public enum TaskStatus { NotStarted, InWork, Completed };

        public event PropertyChangedEventHandler PropertyChanged;

        public Task() { }

        public Task(User author)
        {
            CreationDate = DateTime.Now;
            Author = author;
            CompletePercentage = 0;
            Status = TaskStatus.NotStarted;
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
