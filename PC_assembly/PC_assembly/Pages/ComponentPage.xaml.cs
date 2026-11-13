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
    public partial class ComponentPage : Page
    {
        private ObservableCollection<Component> _list;
        private Component _item;
        private Component? _original;

        public ComponentPage(ObservableCollection<Component> list, Component? component = null)
        {
            _original = component;

            _item = component == null ? new Component() : new Component
            {
                Name = component.Name,
                Type = component.Type,
                Price = component.Price
            };

            _list = list;
            InitializeComponent();
            DataContext = _item;
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_item.Name))
            {
                MessageBox.Show("Введите название компонента");
                return;
            }
            if (string.IsNullOrEmpty(_item.Type))
            {
                MessageBox.Show("Введите тип компонента");
                return;
            }
            if (Validation.GetHasError(PriceBox))
            {
                MessageBox.Show("Цена должна быть числом");
                return;
            }
            if (_item.Price < 0)
            {
                MessageBox.Show("Цена не может быть отрицательной");
                return;
            }

            if (_original == null)
            {
                _list.Add(_item);
                BackClick(sender, e);
                return;
            }

            _original.Name = _item.Name;
            _original.Type = _item.Type;
            _original.Price = _item.Price;
            BackClick(sender, e);
        }
    }
}