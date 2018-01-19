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

namespace _2ModuleTaskTracker.UserManagement {
	/// <summary>
	/// Логика взаимодействия для UserManagementWindow.xaml
	/// </summary>
	public partial class UserManagementWindow : Window {
		public static bool IsShown { get; private set; }

		private Database<User> UserDatabase { get; } = DatabaseManager.Instance.GetDatabase<User>();

		public UserManagementWindow() {
			InitializeComponent();

			NotifyListChanged();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			IsShown = true;
		}

		private void Window_Closed(object sender, EventArgs e) {
			IsShown = false;
		}

		private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
			new UserEditWindow() {
				User = (sender as ListBoxItem).Content as User
			}.ShowDialog();

			NotifyListChanged();
		}

		private void AddUserButton_Click(object sender, RoutedEventArgs e) {
			new UserEditWindow() {
				User = null
			}.ShowDialog();

			NotifyListChanged();
		}

		private void RemoveUserButton_Click(object sender, RoutedEventArgs e) {
			if(listUsers.SelectedItem != null) {
				var user = listUsers.SelectedItem as User;
				if(UserDatabase.Remove(user)) {
					NotifyListChanged();
				}
			}
		}

		private void NotifyListChanged() {
			listUsers.ItemsSource = null;
			listUsers.ItemsSource = UserDatabase.Select();
		}
	}
}
