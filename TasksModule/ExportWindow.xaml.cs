using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace TasksModule
{
    /// <summary>
    /// Логика взаимодействия для ExportWindow.xaml
    /// </summary>
    public partial class ExportWindow : Window
    {
        ObservableCollection<Task> Tasks { get; set; }
        List<Responsible> Responsibles { get; set; }

        private ExportWindow() : this(null, null) { }

        public ExportWindow(ObservableCollection<Task> tasks, List<Responsible> respnsibles)
        {
            InitializeComponent();
            Tasks = tasks;
            Responsibles = respnsibles;
            dpFrom.SelectedDate = dpTo.SelectedDate = DateTime.Now;
        }

        private void BSave_Click(object sender, RoutedEventArgs e)
        {
            if (Tasks == null || Responsibles == null)
            {
                MessageBox.Show("Tasks or responsibles is null.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
            SaveFileDialog dialog = new SaveFileDialog { Filter = "JSON|*.json" };
            try
            {
                if (dialog.ShowDialog() == true)
                {
                    List<Task> tasksToExport =
                        Tasks.Where(t => t.EndDate >= dpFrom.SelectedDate.Value && t.EndDate <= dpTo.SelectedDate.Value).ToList();
                    new JsonWorker().SaveTaskInformation(new ObservableCollection<Task>(tasksToExport), Responsibles, dialog.FileName);
                    MessageBox.Show("Exported tasks count: " + tasksToExport.Count, "Export", MessageBoxButton.OK,MessageBoxImage.Information);
                }
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void BCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
