using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
using Notifications.Wpf.ViewModels.Base;

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
            DataContext = new NetworkDisplayViewModel();

            var viewModel = (NetworkDisplayViewModel)DataContext;
            viewModel.LineDrawRequested += ViewModel_LineDrawRequested;
            viewModel.RemoveAllLinesRequested += ViewModel_RemoveAllLinesRequested;
        }
        private void PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement element && element.DataContext is Entity entity)
            {
                DragDrop.DoDragDrop(element, entity, DragDropEffects.Move);
            }
        }

        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = e.Data.GetDataPresent(typeof(Entity)) ? DragDropEffects.Move : DragDropEffects.None;
            e.Handled = true;
        }

        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            var canvas = (Canvas)sender;
            var viewModel = (NetworkDisplayViewModel)this.DataContext;

            if (e.Data.GetDataPresent(typeof(Entity)))
            {
                var entity = (Entity)e.Data.GetData(typeof(Entity));
                viewModel.HandleDrop(entity, canvas.Name);
            }
        }


        private void ViewModel_LineDrawRequested(object sender, Tuple<Point, Point> e)
        {
            // Draw a line between the two points
            var line = new Line
            {
                X1 = e.Item1.X,
                Y1 = e.Item1.Y,
                X2 = e.Item2.X,
                Y2 = e.Item2.Y,
                Stroke = Brushes.Black,
                StrokeThickness = 2,
            };

            OverlayCanvas.Children.Add(line);
        }

        private void ViewModel_RemoveAllLinesRequested(object sender, EventArgs e)
        {
            OverlayCanvas.Children.Clear();
        }


    }
}
