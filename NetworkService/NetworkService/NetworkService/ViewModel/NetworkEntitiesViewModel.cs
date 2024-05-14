﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using NetworkService.Model;

namespace NetworkService.ViewModel
{
    public class NetworkEntitiesViewModel : BindableBase
    {
        private string _idNumber;
        private string _nameText;
        private string _imagePath;
        private EntityType _typeText;
        private double _value;
        private Entity _selectedEntity;
        private bool _searchByName = true;
        private string _searchTextBoxText;

        public string IdNumber
        {
            get
            {
                return _idNumber;
            }
            set
            {
                if(_idNumber != value)
                {
                    _idNumber = value;
                    OnPropertyChanged(nameof(IdNumber));
                }
            }
        }

        public string NameText
        {
            get
            {
                return _nameText;
            }
            set
            {
                if (_nameText != value)
                {
                    _nameText = value;
                    OnPropertyChanged(nameof(NameText));
                }
            }
        }

        public string ImagePath
        {
            get
            {
                return _imagePath;
            }
            set
            {
                if (_imagePath != value)
                {
                    _imagePath = value;
                    OnPropertyChanged(nameof(ImagePath));
                }
            }
        }

        public EntityType TypeText
        {
            get
            {
                return _typeText;
            }
            set
            {
                if (_typeText != value)
                {
                    _typeText = value;
                    OnPropertyChanged(nameof(TypeText));
                }
            }
        }

        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                if(_value != value)
                {
                    _value = value;
                    OnPropertyChanged(nameof(Value));
                }
            }
        }

        public Entity SelectedEntity
        {
            get { return _selectedEntity; }
            set
            {
                if (_selectedEntity != value)
                {
                    _selectedEntity = value;
                    OnPropertyChanged(nameof(SelectedEntity));
                }
            }
        }

        public bool SearchByName
        {
            get { return _searchByName; }
            set
            {
                if (_searchByName != value)
                {
                    _searchByName = value;
                    OnPropertyChanged(nameof(SearchByName));
                }
            }
        }

        public string SearchTextBoxText
        {
            get { return _searchTextBoxText; }
            set
            {
                if (_searchTextBoxText != value)
                {
                    _searchTextBoxText = value;
                    OnPropertyChanged(nameof(SearchTextBoxText));
                }
            }
        }


        public IEnumerable<EntityType> Types
        {
            get
            {
                return(IEnumerable<EntityType>)Enum.GetValues(typeof(EntityType));
            }
        }

        public ObservableCollection<Entity> Entities { get; set; }

        public MyICommand AddCommand { get; set; }
        public MyICommand DeleteCommand { get; set; }

        public MyICommand AddEntityCommand { get; private set; }
        public MyICommand<string> SearchCommand { get; private set; }

        public NetworkEntitiesViewModel()
        {
            AddCommand = new MyICommand(OnAdd);
            DeleteCommand = new MyICommand(OnDelete, CanDelete);
            SearchCommand = new MyICommand<string>(PerformSearch);
        }

        private void PerformSearch(object parameter)
        {
            var searchText = parameter as string;
            if (string.IsNullOrEmpty(searchText))
            {
                Entities = MainWindowViewModel.Entities;
            }
            else
            {
                if (SearchByName)
                {
                    Entities = new ObservableCollection<Entity>(
                        MainWindowViewModel.Entities.Where(e => e.Name.ToLower().Contains(searchText.ToLower())));
                }
                else
                {
                    Entities = new ObservableCollection<Entity>(
                        MainWindowViewModel.Entities.Where(e => e.EntityType.ToString().ToLower().Contains(searchText.ToLower())));
                }
            }
        }

        private string _idError;
        public string IdError
        {
            get 
            { 
                return _idError; 
            }
            set 
            { 
                _idError = value;
                OnPropertyChanged(nameof(IdError));
            }
        }

        private string _nameError;
        public string NameError
        {
            get
            {
                return _nameError;
            }
            set
            {
                _nameError = value;
                OnPropertyChanged(nameof(NameError));
            }
        }

        private string _imageError;
        public string ImageError
        {
            get
            {
                return _imageError;
            }
            set
            {
                _imageError = value;
                OnPropertyChanged(nameof(ImageError));
            }
        }

        private void OnAdd() {
            List<string> errors = new List<string>();

            if (string.IsNullOrEmpty(IdNumber))
            {
                errors.Add("ID is required.");
            }
            else if (!int.TryParse(IdNumber, out int id))
            {
                errors.Add("ID must be a number.");
            }

            if (string.IsNullOrEmpty(NameText))
            {
                errors.Add("Name is required.");
            }

            if (string.IsNullOrEmpty(ImagePath))
            {
                errors.Add("Image is required.");
            }

            // Update error properties
            IdError = errors.FirstOrDefault(e => e.Contains("ID")) ?? null;
            NameError = errors.FirstOrDefault(e => e.Contains("Name")) ?? null;
            ImageError = errors.FirstOrDefault(e => e.Contains("Image")) ?? null;

            // Add any additional error handling logic here

            // If there are any errors, return without adding the entity
            if (errors.Any())
            {
                return;
            }

            MainWindowViewModel.Entities.Add(new Entity
            {
                Id = int.Parse(IdNumber),
                Name = NameText,
                ImagePath = ImagePath,
                EntityType = TypeText,
                Value = 0
            });
            ResetFormFields();         
        }

        private void ResetFormFields()
        {
            IdNumber = string.Empty;
            NameText = string.Empty;
            ImagePath = null;
            TypeText = EntityType.IntervalMeter;
            ImagePath = null;
        }

        private void OnDelete()
        {
            MainWindowViewModel.Entities.Remove(SelectedEntity);
        }

        private bool CanDelete() { 
            return SelectedEntity != null;
        }

        public void SetImage(string imagePath)
        {
            ImagePath = imagePath;
        }

   

    }
}
