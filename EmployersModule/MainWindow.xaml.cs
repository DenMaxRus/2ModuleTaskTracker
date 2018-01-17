﻿using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CommonLibrary.database;
using CommonLibrary.entities;
using Microsoft.Win32;
using Newtonsoft.Json; 

namespace EmployersModule {
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public static bool IsShown { get; private set; }

		private List<Employer> employees { get; set; }
        private SaveFileDialog saveFileDialog;
        private OpenFileDialog openFileDialog;


        public MainWindow() {
			InitializeComponent();

            saveFileDialog = new SaveFileDialog {
                Filter = "Json|*.json",
                Title = "Select a Json file",
                FileName = "employers.json"
            };
            openFileDialog = new OpenFileDialog {
                Filter = "Json|*.json",
                Title = "Select a Json file",
                FileName = "employers.json"
            };
		}

		private void ExportMenuItem_Click(object sender, RoutedEventArgs e) {
			if(saveFileDialog.ShowDialog() == true) {
				File.WriteAllText(saveFileDialog.FileName, JsonConvert.SerializeObject(employees));
			}
		}

		private void Window_Closed(object sender, EventArgs e) {
			IsShown = false;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			IsShown = true;
		}

        private void ImportMenuItem_Click (object sender, RoutedEventArgs e) {
            Import();
        }

        private void Import () {
            if (openFileDialog.ShowDialog() == true) {
                var employersDatabase = Database<Employer>.Read(openFileDialog.FileName);
                DatabaseManager.Instance.RegisterDatabase(employersDatabase);
                employees = employersDatabase.Select();
                gridEmployers.ItemsSource = employees;
            }
        }
    }
}