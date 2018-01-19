using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static TasksModule.Task;
using CommonLibrary.entities;

namespace TasksModule
{
    /// <summary>
    /// Логика взаимодействия для TaskEditWindow.xaml
    /// </summary>
    public partial class TaskEditWindow : Window
    {
        static string[] statuses = { "Status: not started" , "Status: in work", "Status: completed" };
        public List<Responsible> Responsibles { get; set; }

        public TaskEditWindow() : this(new List<Responsible>() { new Responsible(-1, "Unknown")}) { }

        public TaskEditWindow(List<Responsible> responsibles)
        {
            InitializeComponent();
            Responsibles = responsibles;
            cbResponsible.ItemsSource = Responsibles;
            if (DataContext != null && DataContext is Task)
            {
                tbName.Text = (DataContext as Task).Name;
                tbDuration.Text = (DataContext as Task).Duration.ToString();
                slCompleteRecentage.Value = (DataContext as Task).CompletePercentage;
                dpStartDate.SelectedDate = (DataContext as Task).StartDate;
                dpEndDate.SelectedDate = (DataContext as Task).EndDate;
                tbStatus.Text = statuses[(int)(DataContext as Task).Status];
                tbDescription.Text = (DataContext as Task).Description;
                tbAuthor.Text = "Author: " + (DataContext as Task).Author.Login;
                tbCreationDate.Text = "Creation date: " + (DataContext as Task).CreationDate.ToShortDateString();
            }
            else
            {
                //tbAuthor.Text = "Author: " + Authentication.Instance.CurrentUser.Login;
            }
        }

        private void SlCompleteRecentage_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (e.NewValue == 0)
                tbStatus.Text = statuses[(int)TaskStatus.NotStarted];
            else if (e.NewValue == 100)
                tbStatus.Text = statuses[(int)TaskStatus.Completed];
            else
                tbStatus.Text = statuses[(int)TaskStatus.InWork];
        }
    }
}
