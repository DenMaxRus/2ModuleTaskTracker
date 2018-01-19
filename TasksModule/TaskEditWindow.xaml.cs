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
        static string[] statuses = { "Status: not started", "Status: in work", "Status: completed" };
        public List<Responsible> Responsibles { get; set; }
        public Task Task { get; private set; }

        private TaskEditWindow() : this(new List<Responsible>() { new Responsible(-1, "Unknown") }, new Task(Authentication.Instance.CurrentUser)) { }
        public TaskEditWindow(Task t) : this(new List<Responsible>() { new Responsible(-1, "Unknown") }, t) { }

        /// <summary>
        /// Creates new task edit window. It's only edit so if you need to create task you need to do it before creating this window and send this task here.
        /// </summary>
        /// <param name="responsibles">List of emploees</param>
        /// <param name="task">Task for edit</param>
        public TaskEditWindow(List<Responsible> responsibles, Task task)
        {
            InitializeComponent();
            Responsibles = responsibles;
            Task = task;
            cbResponsible.ItemsSource = Responsibles;
            if (Task != null)
            {
                if (Task.Name != null)
                    tbName.Text = Task.Name;
                if (Task.Duration != double.NaN)
                    tbDuration.Text = Task.Duration.ToString();
                if (Task.CompletePercentage >= 0)
                    slCompleteRecentage.Value = Task.CompletePercentage;
                else
                    slCompleteRecentage.Value = Task.CompletePercentage = 0;
                if (Task.Responsible == null)
                    Task.Responsible = new Responsible(-1, "Unknown");
                cbResponsible.SelectedItem = Task.Responsible;
                if (Task.StartDate != null && Task.StartDate != DateTime.MinValue)
                    dpStartDate.SelectedDate = Task.StartDate;
                else
                    dpStartDate.SelectedDate = Task.StartDate = DateTime.Now;
                if (Task.EndDate != null && Task.EndDate != DateTime.MinValue)
                    dpEndDate.SelectedDate = Task.EndDate;
                else
                    dpEndDate.SelectedDate = Task.EndDate = DateTime.Now;
                tbStatus.Text = statuses[(int)Task.Status];
                if (Task.Description != null)
                    tbDescription.Text = Task.Description;
                tbAuthor.Text = "Author: " + Task.Author.Login;
                tbCreationDate.Text = "Creation date: " + Task.CreationDate.ToShortDateString();
            }
            else
            {
                throw new ArgumentNullException("Task");
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

        private void BSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Task == null)
                {
                    throw new ArgumentNullException("Task");
                }
                else
                {
                    if (Task.Name != null && Task.Name != string.Empty)
                    {
                        Task.Name = tbName.Text;
                        Task.Duration = Convert.ToDouble(tbDuration.Text);
                        Task.CompletePercentage = (int)slCompleteRecentage.Value;
                        Task.StartDate = dpStartDate.SelectedDate.Value;
                        Task.EndDate = dpEndDate.SelectedDate.Value;
                        Task.Description = tbDescription.Text;
                        Task.Responsible = cbResponsible.SelectedItem is Responsible ? cbResponsible.SelectedItem as Responsible : new Responsible(-1, "Unknown");
                        Close();
                    }
                    else
                        MessageBox.Show("Wrong task name", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateEndDate()
        {
            if (dpStartDate.SelectedDate != null && dpStartDate.SelectedDate.Value != DateTime.MinValue &&
                cbResponsible.SelectedItem is Responsible && Responsibles.Contains(cbResponsible.SelectedItem as Responsible) &&
                tbDuration.Text != string.Empty && double.TryParse(tbDuration.Text, out double duration) && duration >= 0)
            {
                dpEndDate.SelectedDate = CalendarManager.GetTaskEndTime(
                    dpStartDate.SelectedDate.Value,
                    CalendarManager.GetDaysQuantity(
                        (cbResponsible.SelectedItem as Responsible).HoursPerDay, duration));
            }
        }

        private void CbResponsible_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateEndDate();

        private void DpStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e) => UpdateEndDate();

        private void TbDuration_TextChanged(object sender, TextChangedEventArgs e) => UpdateEndDate();

        private static bool IsValidDuration(string text)
        {
            bool parsed = double.TryParse(text, out double res);
            return parsed && res >= 0;
        }

        private void TbDuration_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsValidDuration(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void BCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TbDuration_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValidDuration(e.Text);
        }
    }
}
