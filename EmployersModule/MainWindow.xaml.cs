using System;
using System.Collections.Generic;
using System.IO;
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
using CommonLibrary.database;
using CommonLibrary.entities;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace EmployersModule {
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public static bool IsShown { get; private set; }

		private List<Employee> Employees { get; set; }

		public MainWindow() {
			InitializeComponent();

			var employeeDatabase = DatabaseManager.Instance.GetDatabase<Employee>();
			if(employeeDatabase == null) {
				employeeDatabase = Database<Employee>.Create(null);
				DatabaseManager.Instance.RegisterDatabase(employeeDatabase);
			}

			Employees = employeeDatabase.Select();
		}

		private void ExportMenuItem_Click(object sender, RoutedEventArgs e) {
			Export();
		}

		private void ImportMenuItem_Click(object sender, RoutedEventArgs e) {
			Import();
		}

		private void ExportReportMenuItem_Click(object sender, RoutedEventArgs e) {
			MakeReport();
		}


		private void Window_Closed(object sender, EventArgs e) {
			IsShown = false;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			IsShown = true;
		}

		private void Export() {
			var saveFileDialog = new SaveFileDialog {
				Title = "Select file",
				FileName = "employees.json"
			};
			if(saveFileDialog.ShowDialog() == true) {
				File.WriteAllText(saveFileDialog.FileName, JsonConvert.SerializeObject(Employees));
			}
		}

		private void Import() {
			var openFileDialog = new OpenFileDialog {
				Filter = "Json|*.json",
				Title = "Select a Json file",
			};
			if(openFileDialog.ShowDialog() == true) {
				var employersDatabase = DatabaseManager.Instance.GetDatabase<Employee>();
				employersDatabase.Read(openFileDialog.FileName);
				
				gridEmployers.ItemsSource = Employees;
			}
		}

		private void MakeReport() {
			var saveFileDialog = new SaveFileDialog {
				Filter = "Json|*.json",
				Title = "Select file",
				FileName = "report.json"
			};
			if(saveFileDialog.ShowDialog() == true) {
				File.WriteAllText(saveFileDialog.FileName, JsonConvert.SerializeObject(Employees.Select(e => new { e.Id, e.Name })));
			}
		}

		private void ImportReportMenuItem_Click(object sender, RoutedEventArgs e) {
			if(Employees == null || Employees.Count == 0) {
				MessageBox.Show("Загрузите сотрудников", "Ошибка создания отчёта", MessageBoxButton.OK, MessageBoxImage.Error);
			} else {
				new ReportWindow().ShowDialog();
			}

		}
	}
}
