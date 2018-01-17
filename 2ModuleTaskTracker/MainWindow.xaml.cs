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
using System.Windows.Shapes;
using CommonLibrary.database;

namespace _2ModuleTaskTracker {
	/// <summary>
	/// Логика взаимодействия для MenuWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
		}

		private void QuitMenuItem_Click(object sender, RoutedEventArgs e) {
			Close();
		}

		private void UserManagementMenuItem_Click(object sender, RoutedEventArgs e) {
			// TODO Open user management window
		}

		private void Module1Button_Click(object sender, RoutedEventArgs e) {
			// TODO Open module 1 window
			MessageBox.Show("Запуск модуля 1", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		private void Module2Button_Click(object sender, RoutedEventArgs e) {
			// TODO Open module 1 window
			MessageBox.Show("Запуск модуля 2", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		private void Window_Closed(object sender, EventArgs e) {
			DatabaseManager.Instance.Close();
			new AuthenticationWindow().Show();
		}
	}
}
