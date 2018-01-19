using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CommonLibrary.database;
using CommonLibrary.entities;

namespace _2ModuleTaskTracker {
	/// <summary>
	/// Логика взаимодействия для UserManagementWindow.xaml
	/// </summary>
	public partial class UserManagementWindow : Window {
		public static bool IsShown { get; private set; }
		
		public List<User> Users => DatabaseManager.Instance.GetDatabase<User>().Select();

		public UserManagementWindow() {
			InitializeComponent();

			gridUsers.ItemsSource = Users;

            // Не знаю как называется поле допускающее редактирование 
            //gridUsers.EditEnabled = Authentication.Instance.CurrentUser.IsHaveAccessTo("UserManagement", "Edit");
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			IsShown = true;
		}

		private void Window_Closed(object sender, EventArgs e) {
			IsShown = false;
		}
	}
}
