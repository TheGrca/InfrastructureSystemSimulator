using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkService.Model;

namespace NetworkService.ViewModel
{
    public class NetworkEntitiesViewModel : BindableBase
    {
        private string _idNumber;
        private string _nameText;
        private string _imagePath;
        private EntityType _typeText;
        private bool _isNameSelected = true;

        private Entity _selectedEntity;

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

        public bool IsNameSelected
        {
            get
            {
                return _isNameSelected;
            }
            set
            {
                if(_isNameSelected != value)
                {
                    _isNameSelected = value;
                    OnPropertyChanged(nameof(IsNameSelected));
                   //Za pretragu
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

        public Entity SelectedEntity
        {
            get
            {
                return _selectedEntity;
            }
            set
            {
                if(_selectedEntity != value)
                {
                    _selectedEntity = value;
                    OnPropertyChanged(nameof(SelectedEntity));
                    DeleteCommand.RaiseCanExecuteChanged();

                }
            }
        }

        public ObservableCollection<Entity> Entities { get; set; }

        public MyICommand AddCommand { get; set; }
        public MyICommand DeleteCommand { get; set; }

        public NetworkEntitiesViewModel()
        {
            LoadData();
            AddCommand = new MyICommand(OnAdd);
            DeleteCommand = new MyICommand(OnDelete, CanDelete);
        }

        private void LoadData()
        {
            Entities = new ObservableCollection<Entity>();
            Entities.Add(new Entity
            {
                Id = 1,
                Name = "Naziv",
                ImagePath = @"\Resources\Pictures\1.jpg",
                EntityType = EntityType.IntervalMeter
            });
            Entities.Add(new Entity
            {
                Id = 2,
                Name = "Naziv2",
                ImagePath = @"\Resources\Pictures\2.jpg",
                EntityType = EntityType.IntervalMeter
            });
            Entities.Add(new Entity
            {
                Id = 3,
                Name = "Naziv3",
                ImagePath = @"\Resources\Pictures\3.jpg",
                EntityType = EntityType.IntervalMeter
            });
            Entities.Add(new Entity
            {
                Id = 4,
                Name = "Nazi4",
                ImagePath = @"\Resources\Pictures\4.png",
                EntityType = EntityType.SmartMeter
            });
        }

        private void OnAdd() {
            Entities.Add(new Entity
            {
                Id = int.Parse(IdNumber),
                Name = NameText,
                ImagePath = ImagePath,
                EntityType = TypeText
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
            Entities.Remove(SelectedEntity);
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
