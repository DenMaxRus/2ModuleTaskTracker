using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

namespace TasksModule
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Responsible> Responsibles { get; set; }
        public ObservableCollection<Task> Tasks { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        OpenFileDialog openFileDialog;
        SaveFileDialog saveFileDialog;
        JsonWorker worker;

        public MainWindow()
        {
            InitializeComponent();
            saveFileDialog = new SaveFileDialog() { Filter = "JSON files|*.json" };
            openFileDialog = new OpenFileDialog() { Filter = "JSON files|*.json" };
            Tasks = new ObservableCollection<Task>();
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
                SeriesCollection = new SeriesCollection();
                Tasks = worker.LoadTasksFromJson(openFileDialog.FileName);
                foreach (Task t in Tasks)
                {
                    t.PropertyChanged += Task_PropertyChanged;
                    if (!Responsibles.Contains(t.Responsible))
                        t.Responsible = new Responsible(-1, "Unknown");
                }
                dgTasks.ItemsSource = Tasks;
                cbResponsible.ItemsSource = Responsibles;
                Tasks.CollectionChanged += new NotifyCollectionChangedEventHandler(TasksCollectionChanged);

                //Chart
                UpdateChart();
            }
        }

        private void Task_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Duration")
                UpdateChart();
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

        private void UpdateChart()
        {
            SeriesCollection.Clear();
            Dictionary<Responsible, double> assigments = new Dictionary<Responsible, double>();
            foreach (Responsible r in Responsibles)
            {
                assigments.Add(r, 0);
                foreach (Task t in Tasks)
                {
                    if (r.Equals(t.Responsible))
                    {
                        assigments[r] += t.Duration;
                    }
                }
            }
            foreach (KeyValuePair<Responsible, double> kvp in assigments)
            {
                SeriesCollection.Add(new ColumnSeries { Title = kvp.Key.ToString(), Values = new ChartValues<double> { kvp.Value } });
            }
            DurationChart.Series = SeriesCollection;
            DurationChart.AxisX[0].Title = "Responibles";
            DurationChart.AxisY[0].Title = "Summary tasks duration";
        }

        private void TasksCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems.Count == 1 && e.NewItems[0] is Task)
                (e.NewItems[0] as Task).PropertyChanged += Task_PropertyChanged;
            UpdateChart();
        }
    }
}
