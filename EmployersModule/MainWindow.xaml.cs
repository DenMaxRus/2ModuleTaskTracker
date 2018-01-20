using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using CommonLibrary.database;
using CommonLibrary.entities;
using CommonLibrary.ModuleFramework;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace EmployersModule {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public static bool IsShown { get; private set; }

        public static Module Module { get; } = new Module() {
            Name = "EmployeeManagement",
            Actions = new List<string> {
                "EmployeeManagement" + ".READ",
                "EmployeeManagement" + ".WRITE",
            }
        };

        private IEnumerable<Employee> Employees { get; set; }

        public Database<Employee> EmployeeDatabase { get; private set; }
        public User CurrentUser { get; } = Authentication.Instance.CurrentUser;

        public MainWindow () {
            InitializeComponent();

            EmployeeDatabase = DatabaseManager.Instance.GetDatabase<Employee>();
            if (EmployeeDatabase == null) {
                EmployeeDatabase = new Database<Employee>();
                DatabaseManager.Instance.RegisterDatabase(EmployeeDatabase);
            }

            Employees = EmployeeDatabase.Select();

            if (!CurrentUser.IsHaveAccessTo("EmployeeManagement", "EmployeeManagement.WRITE")) {
                Editor.Visibility = Visibility.Collapsed;
                return;
            }
        }

        #region layout events
        private void Window_Closed (object sender, EventArgs e) {
            IsShown = false;
        }

        private void Window_Loaded (object sender, RoutedEventArgs e) {
            IsShown = true;
        }

        private void ExportMenuItem_Click (object sender, RoutedEventArgs e) {
            ExportEmployees();
        }

        private void ImportMenuItem_Click (object sender, RoutedEventArgs e) {
            ImportEmployees();
        }

        private void ExportReportMenuItem_Click (object sender, RoutedEventArgs e) {
            MakeReport();
        }

        private void ImportReportMenuItem_Click (object sender, RoutedEventArgs e) {
            ImportReport();
        }

        private void Add_MenuItem_Click (object sender, RoutedEventArgs e) {
            new AddChangeEmployeeWindow() {
                Employee = null
            }.ShowDialog();

            NotifyListChanged();
        }

        private void Change_MenuItem_Click (object sender, RoutedEventArgs e) {
            if (listEmployees.SelectedIndex != -1) {
                new AddChangeEmployeeWindow() {
                    Employee = listEmployees.SelectedItem as Employee
                }.ShowDialog();

                NotifyListChanged();
            }
        }

        private void Delete_MenuItem_Click (object sender, RoutedEventArgs e) {
            if (listEmployees.SelectedIndex != -1) {
                Employee item = listEmployees.SelectedItem as Employee;
                EmployeeDatabase.Remove(item);
                listEmployees.ItemsSource = null;
                listEmployees.ItemsSource = EmployeeDatabase.Select();
            }
        }
        #endregion

        private void ExportEmployees () {
            var saveFileDialog = new SaveFileDialog {
                Title = "Select file",
                FileName = "employees.json"
            };
            if (saveFileDialog.ShowDialog() == true) {
                File.WriteAllText(saveFileDialog.FileName, JsonConvert.SerializeObject(Employees));
            }
        }

        private void ImportEmployees () {
            var openFileDialog = new OpenFileDialog {
                Filter = "Json|*.json",
                Title = "Select a Json file",
            };
            if (openFileDialog.ShowDialog() == true) {
                var employersDatabase = DatabaseManager.Instance.GetDatabase<Employee>();
                try {
                    employersDatabase.Read(openFileDialog.FileName);
                    gridEmployers.ItemsSource = Employees;
                    listEmployees.ItemsSource = Employees;
                } catch (Exception e) {
                    MessageBox.Show("Не удается загрузить файл");
                }
            }
        }

        private void MakeReport () {
            var saveFileDialog = new SaveFileDialog {
                Filter = "Json|*.json",
                Title = "Select file",
                FileName = "report.json"
            };
            if (saveFileDialog.ShowDialog() == true) {
                File.WriteAllText(saveFileDialog.FileName, JsonConvert.SerializeObject(Employees.Where(e => e.HasWorkOpportunity).Select( e => new { e.Id, e.Name })));
            }
        }

        private void ImportReport () {
            if (Employees == null || Employees.Count() == 0) {
                MessageBox.Show("Загрузите сотрудников", "Ошибка создания отчёта", MessageBoxButton.OK, MessageBoxImage.Error);
            } else {
                new ReportWindow().ShowDialog();
                Employees = EmployeeDatabase.Select();
            }
        }

        private void NotifyListChanged () {
            listEmployees.ItemsSource = null;
            listEmployees.ItemsSource = EmployeeDatabase.Select();
        }
    }
}