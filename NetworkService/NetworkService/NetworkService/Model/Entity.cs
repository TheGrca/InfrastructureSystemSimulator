using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Model
{
    public enum Type
    {
        IntervalMeter,
        SmartMeter
    }
    public class Entity : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private string image;
        private Type type;

        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                if(id != value)
                {
                    id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if(name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string Image
        {
            get
            {
                return image;
            }
            set
            {
                if(image != value)
                {
                    image = value;
                    OnPropertyChanged("Image");
                }
            }
        }

        public Type Type
        {
            get
            {
                return type;
            }
            set
            {
                if(Type != value)
                {
                    Type = value;
                    OnPropertyChanged("Type");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }
    }
}
