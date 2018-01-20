using CommonLibrary.ModuleFramework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TasksModule {
	/// <summary>
	/// Логика взаимодействия для App.xaml
	/// </summary>
	public partial class App : Application {
        public static Module Module { get; } = new Module()
        {
            Name = "TasksModule",
            Actions = new List<string> {
                "TasksModule" + ".READ",
                "TasksModule" + ".WRITE",
                "TasksModule" + ".CHANGE"
            }
        };
    }
}
