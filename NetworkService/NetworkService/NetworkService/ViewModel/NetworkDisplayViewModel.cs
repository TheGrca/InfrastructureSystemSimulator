using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using NetworkService.Model;

namespace NetworkService.ViewModel
{
    public class NetworkDisplayViewModel : BindableBase
    {
        public ObservableCollection<Entity> Entities { get; set; }
        public NetworkDisplayViewModel()
        {
            Entities = MainWindowViewModel.Entities;
        }

        private bool isDragging = false;
        private Image draggedItem = null;
        private int draggedItemIndex = -1;

        
    }
}
