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

namespace _2ModuleTaskTracker.UserManagement {
	/// <summary>
	/// Логика взаимодействия для UserEditWindow.xaml
	/// </summary>
	public partial class UserEditWindow : Window {

		private User user;
		public User User {
			get => user;
			set {
				user = value == null ? new User("username", "password") : User.Copy(value);
				txtPassword.Password = user.Password;
			}
		}

		public UserEditWindow() {
			InitializeComponent();
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e) {
			user.Password = txtPassword.Password;
			var userDatabase = DatabaseManager.Instance.GetDatabase<User>();
			userDatabase.AddOrUpdate(user);

			Close();
		}

		private void Button_Click(object sender, RoutedEventArgs e) {
			Close();
		}
	}
}
