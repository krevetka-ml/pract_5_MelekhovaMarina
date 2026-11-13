using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PC_assembly
{
    public class Build : INotifyPropertyChanged
    {
        private int _id;
        private string _name = string.Empty;
        private string _purpose = string.Empty;
        private string _owner = string.Empty;
        private ObservableCollection<Component> _components = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public Build()
        {
            Subscribe(_components); 
        }

        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Purpose
        {
            get => _purpose;
            set
            {
                if (_purpose != value)
                {
                    _purpose = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Owner
        {
            get => _owner;
            set
            {
                if (_owner != value)
                {
                    _owner = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Component> Components
        {
            get => _components;
            set
            {
                if (_components != value)
                {
                    Unsubscribe(_components);
                    _components = value;
                    Subscribe(_components);
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(BuildTotal));
                }
            }
        }

        [JsonIgnore]
        public decimal BuildTotal => Components.Sum(c => c.Price);

        private void Subscribe(ObservableCollection<Component> list)
        {
            list.CollectionChanged += OnListChanged;
            foreach (var item in list)
            {
                item.PropertyChanged += OnItemChanged;
            }
        }

        private void Unsubscribe(ObservableCollection<Component> list)
        {
            list.CollectionChanged -= OnListChanged;
            foreach (var item in list)
            {
                item.PropertyChanged -= OnItemChanged;
            }
        }

        private void OnListChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (Component item in e.OldItems)
                {
                    item.PropertyChanged -= OnItemChanged;
                }
            }
            if (e.NewItems != null)
            {
                foreach (Component item in e.NewItems)
                {
                    item.PropertyChanged += OnItemChanged;
                }
            }
            OnPropertyChanged(nameof(BuildTotal));
        }

        private void OnItemChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Component.Price))
            {
                OnPropertyChanged(nameof(BuildTotal));
            }
        }
    }
}