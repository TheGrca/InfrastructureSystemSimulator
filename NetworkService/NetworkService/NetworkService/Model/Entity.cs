﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace NetworkService.Model
{
    public enum EntityType
    {
        IntervalMeter,
        SmartMeter
    }
    public class Entity : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private string imagePath;
        private EntityType type;
        private double value;
        private bool isValid;

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

        public string ImagePath
        {
            get
            {
                return imagePath;
            }
            set
            {
                if(imagePath != value)
                {
                    imagePath = value;
                    OnPropertyChanged("Image");
                }
            }
        }

        public EntityType EntityType
        {
            get
            {
                return type;
            }
            set
            {
                if(type != value)
                {
                    type = value;
                    OnPropertyChanged("Type");
                }
            }
        }

        public double Value
        {
            get
            {
                return value;
            }
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    OnPropertyChanged("Value");
                }
            }
        }

        public bool IsValid
        {
            get
            {
                return isValid;
            }
            set
            {
                if(isValid != value)
                {
                    isValid = value;
                    OnPropertyChanged("Value");
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
