using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Model
{
    public class DragAndDropState
    {
        public string CanvasName { get; set; }
        public ObservableCollection<Entity> Entities { get; set; }



    }
}
