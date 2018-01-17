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

		private List<Employer> Employers { get; set; }

		public MainWindow() {
			InitializeComponent();

			var employersDatabase = Database<Employer>.Read("employers.json");
			DatabaseManager.Instance.RegisterDatabase(employersDatabase);

			Employers = employersDatabase.Select();

			gridEmployers.ItemsSource = Employers;
		}

		private void ExportMenuItem_Click(object sender, RoutedEventArgs e) {
			SaveFileDialog saveFileDialog = new SaveFileDialog {
				Filter = "Json|*.json",
				Title = "Select a Json file",
				FileName = "employers.json"
			};

			if(saveFileDialog.ShowDialog() == true) {
				File.WriteAllText(saveFileDialog.FileName, JsonConvert.SerializeObject(Employers));
			}
		}

		private void Window_Closed(object sender, EventArgs e) {
			IsShown = false;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			IsShown = true;
		}
	}
}
