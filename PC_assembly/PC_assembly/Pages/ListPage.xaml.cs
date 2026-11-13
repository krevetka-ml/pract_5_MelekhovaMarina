using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace PC_assembly.Pages
{
    public partial class ListPage : Page
    {
        public ObservableCollection<Build> Builds { get; set; }

        public Build? SelectedBuild { get; set; }

        public ListPage()
        {
            InitializeComponent();

            string path = AppDomain.CurrentDomain.BaseDirectory + "builds.json";

            if (File.Exists(path))
                Builds = JsonSerializer.Deserialize<ObservableCollection<Build>>(File.ReadAllText(path)) ?? new ObservableCollection<Build>();
            else
                Builds = new ObservableCollection<Build>();

            DataContext = this;
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BuildPage(Builds));
        }

        private void EditClick(object sender, RoutedEventArgs e)
        {
            if (SelectedBuild == null)
            {
                MessageBox.Show("Не выбран элемент списка");
                return;
            }

            NavigationService.Navigate(new BuildPage(Builds, SelectedBuild));
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            if (SelectedBuild == null)
            {
                MessageBox.Show("Сборка не выбрана");
                return;
            }

            bool confirm = MessageBox.Show(
                "Вы действительно хотите удалить запись?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            ) == MessageBoxResult.Yes;

            if (confirm)
                Builds.Remove(SelectedBuild);
        }

        private void SaveJsonClick(object sender, RoutedEventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "builds.json";
            string json = JsonSerializer.Serialize(Builds);
            File.WriteAllText(path, json);
            MessageBox.Show("Данные сохранены в JSON");
        }
    }
}