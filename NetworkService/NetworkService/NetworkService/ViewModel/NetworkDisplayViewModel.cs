using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkService.Model;

namespace NetworkService.ViewModel
{
    public class NetworkDisplayViewModel : BindableBase
    {
        public ObservableCollection<Entity> Entities { get; set; }
        public NetworkDisplayViewModel()
        {
            var networkEntitiesViewModel = new NetworkEntitiesViewModel();

            Entities = networkEntitiesViewModel.Entities;
        }
    }
}
