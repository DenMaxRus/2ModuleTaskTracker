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
using CommonLibrary.entities;

namespace _2ModuleTaskTracker {
	/// <summary>
	/// Логика взаимодействия для MenuWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();

			userManagementMenuItem.IsEnabled = Authentication.Instance.CurrentUser.AccessLevel >= UserAccessLevel.Admin;
		}

		private void QuitMenuItem_Click(object sender, RoutedEventArgs e) {
			Close();
		}

		private void UserManagementMenuItem_Click(object sender, RoutedEventArgs e) {
			if(Authentication.Instance.CurrentUser.AccessLevel >= UserAccessLevel.Admin
				&& !UserManagementWindow.IsShown) {
				new UserManagementWindow().Show();
			}
		}

		private void Module1Button_Click(object sender, RoutedEventArgs e) {
			if(!EmployersModule.MainWindow.IsShown) {
				new EmployersModule.MainWindow().Show();
			}
		}

		private void Module2Button_Click(object sender, RoutedEventArgs e) {
            if (!TasksModule.MainWindow.IsShown)
                try { new TasksModule.MainWindow(Authentication.Instance.CurrentUser).Show(); } catch { }

        }

		private void Window_Closed(object sender, EventArgs e) {
			DatabaseManager.Instance.Close();
			new AuthenticationWindow().Show();
		}
	}
}
