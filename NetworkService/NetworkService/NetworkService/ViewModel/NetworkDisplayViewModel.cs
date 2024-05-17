using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using NetworkService.Model;

namespace NetworkService.ViewModel
{
    public class NetworkDisplayViewModel : BindableBase
    {
        public MyICommand<Canvas> DropEvent { get; set; }
        public MyICommand<Canvas> DragOverEvent { get; set; }
        public MyICommand<Canvas> MouseLeftButtonDownEvent { get; set; }
        public NetworkDisplayViewModel()
        {

        }
    }
}
