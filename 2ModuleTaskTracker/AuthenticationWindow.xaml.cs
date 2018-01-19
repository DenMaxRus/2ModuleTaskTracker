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
using CommonLibrary.ModuleFramework;

namespace _2ModuleTaskTracker {
	/// <summary>
	/// Логика взаимодействия для AuthenticationWindow.xaml
	/// </summary>
	public partial class AuthenticationWindow : Window {

		static AuthenticationWindow() {
			ModuleManager.Instance.Register(new Module() {
				Name = "ALL",
				Actions = new List<string> { "ALL" }
			});
			ModuleManager.Instance.Register(EmployersModule.App.Module);

			var rolesDatabase = new UserRoleDatabase().Read("roles.json");
			if(rolesDatabase.Select().Count == 0) {
				var godRole = new UserRole();
				var godModule = ModuleManager.Instance.Modules["ALL"];
				foreach(var action in godModule.Actions) {
					godRole.SetAccess(godModule.Name, action, true);
				}

				rolesDatabase.Add(godRole);
			}
			DatabaseManager.Instance.RegisterDatabase(rolesDatabase);

			var userDatabase = new UserDatabase().Read("users.json");
			DatabaseManager.Instance.RegisterDatabase(userDatabase);
		}

		public AuthenticationWindow() {
			InitializeComponent();

			Authentication.Instance.Logout();
		}

		private void Login_Click(object sender, RoutedEventArgs e) {
			if(string.IsNullOrEmpty(txtLogin.Text) || string.IsNullOrEmpty(txtPassword.Password)) {
				MessageBox.Show("Пустой логин или пароль", "Ошибка аутентификации", MessageBoxButton.OK, MessageBoxImage.Error);
			} else if(!Authentication.Instance.Login(txtLogin.Text, txtPassword.Password)) {
				MessageBox.Show("Неправильный логин или пароль", "Ошибка аутентификации", MessageBoxButton.OK, MessageBoxImage.Error);
			} else {
				// MessageBox.Show("Успешная авторизация", "Информация об аутентификации", MessageBoxButton.OK, MessageBoxImage.Information);

				new MainWindow().Show();

				Close();
			}
		}
	}
}
