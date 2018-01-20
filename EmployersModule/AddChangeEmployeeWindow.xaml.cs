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

namespace EmployersModule {
    /// <summary>
    /// Логика взаимодействия для AddChangeEmployeeWindow.xaml
    /// </summary>
    public partial class AddChangeEmployeeWindow : Window {

        private Employee employee;
        public Employee Employee {
            get => employee;
            set {
                employee = value == null ? new Employee() : Employee.Copy(value);
                NameBox.Text = employee.Name;
                OccupationBox.Text = employee.Occupation;
                HoursPerDayBox.Text = employee.HoursPerDay.ToString();
                HasWorkOpportunityBox.Text = employee.HasWorkOpportunity.ToString();
                HasWorkOpportunityBox.ItemsSource = new []{true, false};
            }
        }

        public AddChangeEmployeeWindow () {
            InitializeComponent();
        }

        private void Action_Click (object sender, RoutedEventArgs e) {
            employee.Name = NameBox.Text;
            employee.Occupation = OccupationBox.Text;
            employee.HoursPerDay = Convert.ToInt32(HoursPerDayBox.Text);
            employee.HasWorkOpportunity = Convert.ToBoolean(HasWorkOpportunityBox.Text);

            var employeeDatabase = DatabaseManager.Instance.GetDatabase<Employee>();
            employeeDatabase.AddOrUpdate(employee);

            Close();
        }
    }
}
