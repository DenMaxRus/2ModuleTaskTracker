using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TasksModule {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Responsible> Responsibles { get; set; }
        public List<Task> Tasks { get; set; }
        OpenFileDialog openFileDialog;
        SaveFileDialog saveFileDialog;
        JsonWorker worker;

        public MainWindow()
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
                foreach (Task t in Tasks)
                {
                    if (!Responsibles.Contains(t.Responsible))
                        t.Responsible = new Responsible(-1, "Unknown");
                }
                dgTasks.ItemsSource = Tasks;
                cbResponsible.ItemsSource = Responsibles;
            }
        }

        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            if (Tasks != null && Tasks.Count > 0 && Responsibles != null && Responsibles.Count > 0)
            {
                if (saveFileDialog.ShowDialog() == true)
                {
                    worker.SaveTaskInformation(Tasks, Responsibles, saveFileDialog.FileName);
                }
            }
        }
    }
}
