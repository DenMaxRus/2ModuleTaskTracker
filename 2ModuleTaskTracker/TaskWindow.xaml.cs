using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;

namespace _2ModuleTaskTracker
{
    /// <summary>
    /// Логика взаимодействия для TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        public List<string> Responsibles { get; private set; }
        public List<Task> Tasks { get; set; }
        OpenFileDialog openFileDialog;
        SaveFileDialog saveFileDialog;
        JsonWorker worker;

        public TaskWindow()
        {
            InitializeComponent();
            saveFileDialog = new SaveFileDialog();
            openFileDialog = new OpenFileDialog();
            Tasks = new List<Task>();
            Responsibles = new List<string>();
            worker = new JsonWorker();
            dgTasks.ItemsSource = Tasks;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if(saveFileDialog.ShowDialog() == true)
            {
                worker.SaveTasksToJson(Tasks, saveFileDialog.FileName);
            }
        }

        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            if(openFileDialog.ShowDialog() == true)
            {
                Tasks = worker.LoadTasksFromJson(openFileDialog.FileName);
                dgTasks.ItemsSource = Tasks;
            }
        }
    }
}
