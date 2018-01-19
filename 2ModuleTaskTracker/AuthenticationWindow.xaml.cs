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
	/// Логика взаимодействия для AuthenticationWindow.xaml
	/// </summary>
	public partial class AuthenticationWindow : Window {
		public AuthenticationWindow() {
			InitializeComponent();

			Authentication.Instance.Logout();

			var userDatabase = DatabaseManager.Instance.GetDatabase<User>();
			if(userDatabase == null) {
				var UserDatabase = new UserDatabase().Read("users.json");
				DatabaseManager.Instance.RegisterDatabase(UserDatabase);
			}
		}

		private void Login_Click(object sender, RoutedEventArgs e) {
			if(string.IsNullOrEmpty(txtLogin.Text) || string.IsNullOrEmpty(txtPassword.Password)) {
				MessageBox.Show("Пустой логин или пароль", "Ошибка аутентификации", MessageBoxButton.OK, MessageBoxImage.Error);
			} else if(!Authentication.Instance.Login(txtLogin.Text, txtPassword.Password, DatabaseManager.Instance.GetDatabase<User>())) {
				MessageBox.Show("Неправильный логин или пароль", "Ошибка аутентификации", MessageBoxButton.OK, MessageBoxImage.Error);
			} else {
				// MessageBox.Show("Успешная авторизация", "Информация об аутентификации", MessageBoxButton.OK, MessageBoxImage.Information);

				new MainWindow().Show();

				Close();
			}
		}
	}
}
