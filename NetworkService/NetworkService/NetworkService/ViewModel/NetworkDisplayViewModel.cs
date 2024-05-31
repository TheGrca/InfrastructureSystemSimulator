using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using NetworkService.Model;

namespace NetworkService.ViewModel
{
    public class NetworkDisplayViewModel : BindableBase
    {
        private readonly DispatcherTimer _timer;
        public ICommand ClearCanvasCommand { get; }
        public NetworkDisplayViewModel()
        {
            EntitiesTreeView = MainWindowViewModel.EntitiesTreeView;

            CanvasEntities = new Dictionary<string, ObservableCollection<Entity>>();

            for (int i = 1; i <= 12; i++)
            {
                CanvasEntities.Add($"Canvas{i}", new ObservableCollection<Entity>());
            }
            ClearCanvasCommand = new MyICommand<string>(ClearCanvas);

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += (sender, args) => UpdateCanvasBorderColors();
            _timer.Start();
        }
        public Dictionary<string, ObservableCollection<Entity>> CanvasEntities { get; set; }
        public ObservableCollection<EntityByType> EntitiesTreeView { get; set; }



        public void HandleDrop(Entity entity, string canvasName)
        {
            if (EntitiesTreeView == null) throw new InvalidOperationException("EntitiesTreeView is not initialized.");
            if (CanvasEntities == null || !CanvasEntities.ContainsKey(canvasName)) throw new InvalidOperationException($"CanvasEntities does not contain {canvasName}.");

            foreach (var entityByType in EntitiesTreeView)
            {
                if (entityByType.Entities == null) continue;
                if (entityByType.Entities.Contains(entity))
                {
                    entityByType.Entities.Remove(entity);
                    break;
                }
            }
            CanvasEntities[canvasName].Clear();
            CanvasEntities[canvasName].Add(entity);

            OnPropertyChanged(nameof(EntitiesTreeView));
            OnPropertyChanged(nameof(CanvasEntities));
            UpdateCanvasBorderColors();
        }

        private void ClearCanvas(string canvasName)
        {
            if (CanvasEntities.TryGetValue(canvasName, out var entities) && entities.Any())
            {
                var entity = entities.First();
                entities.Clear();

                // Find the appropriate EntityByType to return the entity to
                var entityType = EntitiesTreeView.FirstOrDefault(e => e.Type == entity.EntityType.ToString());
                if (entityType != null)
                {
                    entityType.Entities.Add(entity);
                }
                else
                {
                    EntitiesTreeView.Add(new EntityByType
                    {
                        Type = entity.EntityType.ToString(),
                        Entities = new ObservableCollection<Entity> { entity }
                    });
                }
                MainWindowViewModel.ShowToastNotification(new ToastNotification("Success", "Entity cleared out of canvas!", Notification.Wpf.NotificationType.Success));
                OnPropertyChanged(nameof(CanvasEntities));
                OnPropertyChanged(nameof(EntitiesTreeView));
                UpdateCanvasBorderColors();
            }
        }


        //Lines between canvas

        public ObservableCollection<LineModel> Lines { get; } = new ObservableCollection<LineModel>();

        // Method to add a line between two canvases
        public void AddLine(string startCanvas, string endCanvas)
        {
            // Create a new line with start and end points
            var line = new LineModel(startCanvas, endCanvas);
            Lines.Add(line);
        }

        // Method to remove lines connected to a canvas
        public void RemoveLinesByCanvas(string canvasName)
        {
            var linesToRemove = new List<LineModel>();

            // Find lines where either start or end canvas matches the specified canvasName
            foreach (var line in Lines)
            {
                if (line.StartCanvas == canvasName || line.EndCanvas == canvasName)
                {
                    linesToRemove.Add(line);
                }
            }

            // Remove lines from the collection
            foreach (var lineToRemove in linesToRemove)
            {
                Lines.Remove(lineToRemove);
            }
        }

        // Canvas Value Color
        private Dictionary<string, Brush> _canvasBorderColors = new Dictionary<string, Brush>();
        public Dictionary<string, Brush> CanvasBorderColors
        {
            get { return _canvasBorderColors; }
            set
            {
                _canvasBorderColors = value;
                OnPropertyChanged(nameof(CanvasBorderColors));
            }
        }

        public void UpdateCanvasBorderColors()
        {
            foreach (var canvas in CanvasEntities)
            {
                string canvasName = canvas.Key;
                var entities = canvas.Value;

                if (entities.Count == 0)
                {
                    _canvasBorderColors[canvasName] = Brushes.Transparent;
                }
                else
                {
                    var value = entities.First().Value; 
                    if (value < 0.34 || value > 2.73)
                    {
                        _canvasBorderColors[canvasName] = Brushes.Red;
                    }
                    else
                    {
                        _canvasBorderColors[canvasName] = Brushes.Green;
                    }
                }
            }

            OnPropertyChanged(nameof(CanvasBorderColors));
        }

    }
}
