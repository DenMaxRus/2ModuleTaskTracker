using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CommonLibrary.ModuleFramework;

namespace EmployersModule {
	/// <summary>
	/// Логика взаимодействия для App.xaml
	/// </summary>
	public partial class App : Application {

		public static Module Module { get; } = new Module() {
			Name = "EmployeesModule",
			Actions = new List<string> {
				"EmployeesModule" + ".READ",
				"EmployeesModule" + ".WRITE",
			}
		};
	}
}
