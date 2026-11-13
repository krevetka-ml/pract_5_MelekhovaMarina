using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PC_assembly.Pages
{
    public partial class BuildPage : Page
    {
        private ObservableCollection<Build> _builds;
        private Build? _original;

        public Build Current { get; set; }

        public Component? SelectedComponent { get; set; }

        public BuildPage(ObservableCollection<Build> builds, Build? build = null)
        {
            _original = build;
            _builds = builds;

            if (build == null)
            {
                Current = new Build
                {
                    Id = builds.Count == 0 ? 1 : builds.Max(b => b.Id) + 1
                };
            }
            else
            {
                Current = new Build
                {
                    Id = build.Id,
                    Name = build.Name,
                    Purpose = build.Purpose,
                    Owner = build.Owner,
                    Components = new ObservableCollection<Component>(
                        build.Components.Select(c => new Component
                        {
                            Name = c.Name,
                            Type = c.Type,
                            Price = c.Price
                        }))
                };
            }

            InitializeComponent();
            DataContext = this;
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ComponentPage(Current.Components));
        }

        private void EditClick(object sender, RoutedEventArgs e)
        {
            if (SelectedComponent == null)
            {
                MessageBox.Show("Не выбран элемент списка");
                return;
            }
            NavigationService.Navigate(new ComponentPage(Current.Components, SelectedComponent));
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            if (SelectedComponent == null)
            {
                MessageBox.Show("Компонент не выбран");
                return;
            }
            bool confirm = MessageBox.Show(
                    "Вы действительно хотите удалить запись?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                ) == MessageBoxResult.Yes;

            if (confirm)
            {
                Current.Components.Remove(SelectedComponent);
            }
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (Current.Id <= 0)
            {
                MessageBox.Show("Id должен быть больше нуля");
                return;
            }
            if (string.IsNullOrEmpty(Current.Name))
            {
                MessageBox.Show("Введите название сборки");
                return;
            }
            if (string.IsNullOrEmpty(Current.Purpose))
            {
                MessageBox.Show("Введите назначение сборки");
                return;
            }
            if (string.IsNullOrEmpty(Current.Owner))
            {
                MessageBox.Show("Введите владельца сборки");
                return;
            }

            if (_original == null)
            {
                _builds.Add(Current);
            }
            else
            {
                _original.Id = Current.Id;
                _original.Name = Current.Name;
                _original.Purpose = Current.Purpose;
                _original.Owner = Current.Owner;
                _original.Components = Current.Components;
            }
            BackClick(sender, e);
        }
    }
}