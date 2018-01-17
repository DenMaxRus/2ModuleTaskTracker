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
        public List<Responsible> Responsibles { get; set; }
        public List<Task> Tasks { get; set; }
        OpenFileDialog openFileDialog;
        SaveFileDialog saveFileDialog;
        JsonWorker worker;

        public TaskWindow()
        {
            InitializeComponent();
            saveFileDialog = new SaveFileDialog() { Filter = "JSON files|*.json" };
            openFileDialog = new OpenFileDialog() { Filter = "JSON files|*.json" };
            Tasks = new List<Task>();
            worker = new JsonWorker();
            dgTasks.ItemsSource = Tasks;
            if (MessageBox.Show("Chose file with employers.", "File loading", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Cancel) == MessageBoxResult.OK)
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    Responsibles = worker.LoadResponsiblesFromJson(openFileDialog.FileName);
                }
                else
                    Close();
            }
            else
                Close();
            if (Responsibles != null)
            {
                Responsibles.Add(new Responsible(-1, "Unknown"));
                cbResponsible.ItemsSource = Responsibles;
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (saveFileDialog.ShowDialog() == true)
            {
                worker.SaveTasksToJson(Tasks, saveFileDialog.FileName);
            }
        }

        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            if (openFileDialog.ShowDialog() == true)
            {
                Tasks = worker.LoadTasksFromJson(openFileDialog.FileName);
                dgTasks.ItemsSource = Tasks;
                foreach (Task t in Tasks)
                {
                    if (!Responsibles.Contains(t.Responsible))
                        t.Responsible = new Responsible(-1, "Unknown");
                }
                cbResponsible.ItemsSource = Responsibles;
            }
        }

        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            if(Tasks!= null && Tasks.Count > 0 && Responsibles!= null && Responsibles.Count > 0)
            {
                if (saveFileDialog.ShowDialog() == true)
                {
                    worker.SaveTaskInformation(Tasks, Responsibles, saveFileDialog.FileName);
                }
            }
        }
    }
}
