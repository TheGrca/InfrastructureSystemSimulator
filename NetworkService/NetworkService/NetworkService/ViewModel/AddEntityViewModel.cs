using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkService.Model;
using System.Windows.Input;

namespace NetworkService.ViewModel
{
    public class AddEntityViewModel : BindableBase
    {
        private string _idNumber;
        private string _nameText;
        private EntityType _typeText;
        private string _imagePath;

        public string IdNumber
        {
            get { return _idNumber; }
            set { SetProperty(ref _idNumber, value); }
        }

        public string NameText
        {
            get { return _nameText; }
            set { SetProperty(ref _nameText, value); }
        }

        public EntityType TypeText
        {
            get { return _typeText; }
            set { SetProperty(ref _typeText, value); }
        }

        public string ImagePath
        {
            get { return _imagePath; }
            set { SetProperty(ref _imagePath, value); }
        }

        public IEnumerable<EntityType> Types { get; } = Enum.GetValues(typeof(EntityType)) as IEnumerable<EntityType>;

        public ICommand SelectImageCommand { get; }
        public ICommand AddCommand { get; }

    }
}
