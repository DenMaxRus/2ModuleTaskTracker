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
using System.Windows.Shapes;
using CommonLibrary.database;
using CommonLibrary.entities;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace EmployersModule {
	/// <summary>
	/// Логика взаимодействия для ReportWindow.xaml
	/// </summary>
	public partial class ReportWindow : Window {
		private List<Employee> Employees { get; set; }

		public ReportWindow() {
			InitializeComponent();

			var employeeDatabase = DatabaseManager.Instance.GetDatabase<Employee>();
			Employees = employeeDatabase.Select();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			var openFileDialog = new OpenFileDialog {
				Filter = "Json|*.json",
				Title = "Select a Json file",
			};
			if(openFileDialog.ShowDialog() == true) {
				var employeeSalaryMap = JsonConvert.DeserializeObject<Dictionary<String, double>>(File.ReadAllText(openFileDialog.FileName));
				var employeeSalaryMap2 = employeeSalaryMap.Select(m => {
					var employeeId = Convert.ToInt32(m.Key);
					var employee = Employees.Find(x => x.Id == employeeId);
					return new {
						Employee = new {
							Id = employeeId,
							Name = employee != null ? employee.Name : "unknown",
							Occupation = employee != null ? employee.Occupation : "unknown",
							Salary = employee != null ? employee.Salary.ToString() : "unknown",
						},
						TotalSalary = employee != null ? m.Value * employee.Salary : 0,
					};
				});
				employeeSalaryMap2.Any();
				gridReport.ItemsSource = employeeSalaryMap2;
			}
		}
	}
}
