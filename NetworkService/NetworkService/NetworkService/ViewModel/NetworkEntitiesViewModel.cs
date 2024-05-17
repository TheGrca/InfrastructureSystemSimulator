using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
        private string _searchText;
        private ICollectionView _entitiesView;


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
        private Visibility _isImageVisible = Visibility.Visible; // Default value to true if the image is initially visible
        public Visibility IsImageVisible
        {
            get { return _isImageVisible; }
            set
            {
                if (_isImageVisible != value)
                {
                    _isImageVisible = value;
                    OnPropertyChanged(nameof(IsImageVisible));
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
                    DeleteCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                if(value != _searchText)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    _entitiesView.Refresh();
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
            _entitiesView = CollectionViewSource.GetDefaultView(this);
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
