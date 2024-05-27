using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NetworkService.Model;
using NetworkService.ViewModel;

namespace NetworkService.Views
{
    /// <summary>
    /// Interaction logic for NetworkDisplayView.xaml
    /// </summary>
    public partial class NetworkDisplayView : UserControl
    {
        public NetworkDisplayView()
        {
            InitializeComponent();
        }
        private void TreeView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement element && element.DataContext is Entity entity)
            {
                DragDrop.DoDragDrop(element, entity, DragDropEffects.Move);
            }
        }

        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            var canvas = (Canvas)sender;
            var viewModel = (NetworkDisplayViewModel)this.DataContext;

            if (e.Data.GetDataPresent(typeof(Entity)) && !viewModel.CanvasEntities[canvas.Name].Any())
            {
                e.Effects = DragDropEffects.Move;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Entity)))
            {
                var entity = (Entity)e.Data.GetData(typeof(Entity));
                var canvas = (Canvas)sender;

                var viewModel = (NetworkDisplayViewModel)this.DataContext;
                viewModel.HandleDrop(entity, canvas.Name);

                e.Handled = true;
            }
        }

    }
}
