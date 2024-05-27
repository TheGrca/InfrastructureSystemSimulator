using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
        private string _searchTextBoxText;
        private string _imagePath;
        private EntityType _typeText;
        private double _value;
        private Entity _selectedEntity;
        private string _searchText;
        private ICollectionView _entitiesView;
        private bool _isComboBoxAvailable;
        private bool _isTextBoxAvailable;
        private bool _searchByName;
        private bool _nameTextChanged;
        private bool _searchByType;


        public bool NameTextChanged
        {
            get
            {
                return _nameTextChanged;
            }
            set
            {
                if(_nameTextChanged != value)
                {
                    _nameTextChanged = value;
                    OnPropertyChanged(nameof(NameTextChanged));
                    FilterByName();
                }
            }
        }
        public void FilterByName()
        {
            ObservableCollection<Entity> Entities = MainWindowViewModel.Entities;
            MainWindowViewModel.Entities.Clear();
            foreach(Entity e in Entities)
            {
                MainWindowViewModel.Entities.Add(e);
            }
        }
        public bool IsTextBoxAvailable
        {
            get
            {
                return _isTextBoxAvailable;
            }
            set
            {
                if (_isTextBoxAvailable != value)
                {
                    _isTextBoxAvailable = value;
                    OnPropertyChanged(nameof(IsTextBoxAvailable));
                }
            }
        }

        public bool IsComboBoxAvailable
        {
            get
            {
                return _isComboBoxAvailable;
            }
            set
            {
                if (_isComboBoxAvailable != value)
                {
                    _isComboBoxAvailable = value;
                    OnPropertyChanged(nameof(IsComboBoxAvailable));
                }
            }
        }

        public bool SearchByName
        {
            get
            {
              return _searchByName;
            }
            set
            {
                if (_searchByName != value)
                {
                    _searchByName = value;
                    OnPropertyChanged(nameof(SearchByName));
                    UpdateAvailability();
                }
            }
        }

        public bool SearchByType
        {
            get 
            { 
                return _searchByType;
            }
            set
            {
                if (_searchByType != value)
                {
                    _searchByType = value;
                    OnPropertyChanged(nameof(SearchByType));
                    UpdateAvailability();
                }
            }
        }

        private void UpdateAvailability()
        {
            IsComboBoxAvailable = SearchByType;
            IsTextBoxAvailable = SearchByName;
            if (!IsTextBoxAvailable)
            {
                SearchTextBoxText = "";
            }

            if (!IsComboBoxAvailable)
            {
                SelectedType = null;
            }
        }

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

        public string SearchTextBoxText
        {
            get
            {
                return _searchTextBoxText;
            }
            set
            {
                if (_searchTextBoxText != value)
                {
                    _searchTextBoxText = value;
                    OnPropertyChanged(nameof(SearchTextBoxText));
                    Search();
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

        private string selectedType;
        public string SelectedType
        {
            get
            {
                return selectedType;
            }
            set
            {
                if(selectedType != value)
                {
                    selectedType = value;
                    OnPropertyChanged(nameof(SelectedType));
                    Search();
                }
            }

        }
        public NetworkEntitiesViewModel()
        {
            AddCommand = new MyICommand(OnAdd);
            DeleteCommand = new MyICommand(OnDelete, CanDelete);
            _entitiesView = CollectionViewSource.GetDefaultView(this);
            SearchByName = true;
            SearchByType = false;
            EntitiesList = MainWindowViewModel.Entities;
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


            IdError = errors.FirstOrDefault(e => e.Contains("ID")) ?? null;
            NameError = errors.FirstOrDefault(e => e.Contains("Name")) ?? null;

            if (errors.Any())
            {
                return;
            }

            if (string.IsNullOrEmpty(ImagePath))
            {
                if (TypeText.ToString() == "SmartMeter")
                {
                    ImagePath = "\\Resources\\Pictures\\SmartMeter.png";
                }
                else
                {
                    ImagePath = "\\Resources\\Pictures\\IntervalMeter.png";
                }
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

        private bool DeleteConfirmation()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this entity?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }

        private void OnDelete()
        {
            if (DeleteConfirmation())
            {
                MainWindowViewModel.Entities.Remove(SelectedEntity);
            }
        }

        private bool CanDelete() { 
            return SelectedEntity != null;
        }

        public void SetImage(string imagePath)
        {
            ImagePath = imagePath;
        }


        //Filter
        private ObservableCollection<Entity> _entitiesList;
        public ObservableCollection<Entity> EntitiesList
        {
            get { return _entitiesList; }
            set
            {
                _entitiesList = value;
                OnPropertyChanged(nameof(EntitiesList));
            }
        }

        private void Search()
        {
            List<Entity> entities = new List<Entity>();
            if (SearchByName)
            {
                if (SearchTextBoxText != null)
                {

                    entities = MainWindowViewModel.Entities
                               .Where(entity => entity.Name.ToLower().Contains(SearchTextBoxText.ToLower()))
                               .ToList();

                }
                else { 

                entities = MainWindowViewModel.Entities.ToList();
                }
            }
            else if (SearchByType)
            {
                if (SelectedType != null)
                {
                    EntityType parsedType;
                    if (Enum.TryParse(SelectedType, out parsedType)) {
                        entities = MainWindowViewModel.Entities
                                  .Where(entity => entity.EntityType == parsedType)
                                  .ToList();
                    }
                }
                else
                {
                    entities = MainWindowViewModel.Entities.ToList();
                }
            }
            else
            {
                entities = MainWindowViewModel.Entities.ToList();
            }
            EntitiesList = new ObservableCollection<Entity>(entities);
        }
   

    }
}
