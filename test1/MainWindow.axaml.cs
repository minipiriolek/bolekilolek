using Avalonia.Controls;
using Avalonia.Media.Imaging;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace test1
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<TaskItem> tasks = new ObservableCollection<TaskItem>();

        public MainWindow()
        {
            InitializeComponent();
            TaskListBox.ItemsSource = tasks;
            CharacterComboBox.SelectedIndex = 0;
            UpdateCharacterImage();
        }

        private void UpdateCharacterImage() //nie dziala
        {
            if (CharacterComboBox.SelectedItem is ComboBoxItem item)
            {
                var name = item.Content?.ToString() ?? "";
                var uri = $"avares://test1/Assets/{(name == "Bolek" ? "bolek.png" : "lolek.png")}";
                try { CharacterImage.Source = new Bitmap(uri); } catch { CharacterImage.Source = null; }
            }
        }

        private void OnCharacterChanged(object? sender, SelectionChangedEventArgs e)
        {
            UpdateCharacterImage();
        }

        private void OnAddTaskClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var name = TaskNameBox.Text ?? "";
            if (string.IsNullOrWhiteSpace(name)) return;

            var character = (CharacterComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "???";
            var priority = LowPriority.IsChecked == true ? "łatwe" :
                           NormalPriority.IsChecked == true ? "średnie" :
                           HighPriority.IsChecked == true ? "trudne" : "zwykłe";
            var extras = "";
            if (OutdoorCheckBox.IsChecked == true) extras += " 🌳";
            if (EquipmentCheckBox.IsChecked == true) extras += " 🔧";
            if (FriendsCheckBox.IsChecked == true) extras += " 🤝";
            var date = TaskCalendar.SelectedDate ?? DateTime.Today;

            var task = new TaskItem
            {
                Name = name,
                Character = character,
                Priority = priority,
                Extras = extras,
                Date = date.Date
            };

            tasks.Add(task);
            TaskNameBox.Text = "";
        }

        private void OnRemoveTaskClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItem is TaskItem selected)
            {
                tasks.Remove(selected);
            }
        }

        private void OnShowTasksForDateClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var date = TaskCalendar.SelectedDate ?? DateTime.Today;
            var summary = new SummaryWindow(tasks, date.Date);
            summary.Show();
        }
    }

    public class TaskItem
    {
        public string Name { get; set; } = "";
        public string Character { get; set; } = "";
        public string Priority { get; set; } = "";
        public string Extras { get; set; } = "";
        public DateTime Date { get; set; }

        public override string ToString() => $"{Character} – {Name} [{Priority}] {Extras}";
    }
}
