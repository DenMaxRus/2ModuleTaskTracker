using System;
using System.Collections.Generic;
using System.Globalization;
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
				cmbRole.SelectedItem = AvailableRoles.FirstOrDefault(r => r.Id == user.RoleId);
			}
		}

		private Database<UserRole> RoleDatabase { get; } = DatabaseManager.Instance.GetDatabase<UserRole>();

		public IReadOnlyCollection<UserRole> AvailableRoles { get; private set; }

		public UserEditWindow() {
			InitializeComponent();

			AvailableRoles = RoleDatabase.Select();
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

		private class UserRoleToStringConverter : IValueConverter {
			public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
				return (value as UserRole).Id;
			}

			public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
				throw new NotImplementedException();
			}
		}
	}
}
