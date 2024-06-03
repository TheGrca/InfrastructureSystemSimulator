using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Linq;
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
using NetworkService.Views;

namespace NetworkService.ViewModel
{
    public class NetworkDisplayViewModel : BindableBase
    {
        private readonly DispatcherTimer _timer;
        private Entity _selectedFirstEntity;
        private Entity _selectedSecondEntity;
        public ICommand ClearCanvasCommand { get; }
        public ICommand ConnectCommand { get; }
        public NetworkDisplayViewModel()
        {
            EntitiesTreeView = MainWindowViewModel.EntitiesTreeView;

            CanvasEntities = new Dictionary<string, ObservableCollection<Entity>>();
            EntityConnections = new ObservableCollection<Connection>();

            for (int i = 1; i <= 12; i++)
            {
                CanvasEntities.Add($"Canvas{i}", new ObservableCollection<Entity>());
            }
            ClearCanvasCommand = new MyICommand<string>(ClearCanvas);
            ConnectCommand = new MyICommand(ConnectEntities, CanConnectEntities);

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += (sender, args) => UpdateCanvasBorderColors();
            _timer.Start();
            EntitiesInCanvas = new ObservableCollection<Entity>();
            FirstComboBoxItems = new ObservableCollection<Entity>();
            SecondComboBoxItems = new ObservableCollection<Entity>();
        }
        public Dictionary<string, ObservableCollection<Entity>> CanvasEntities { get; set; }
        public ObservableCollection<EntityByType> EntitiesTreeView { get; set; }
        public ObservableCollection<Entity> EntitiesInCanvas { get; set; }
        public ObservableCollection<Entity> FirstComboBoxItems { get; set; }
        public ObservableCollection<Entity> SecondComboBoxItems { get; set; }
        public ObservableCollection<Connection> EntityConnections { get; set; }

        public Entity SelectedFirstEntity
        {
            get => _selectedFirstEntity;
            set => SetProperty(ref _selectedFirstEntity, value);
        }
        public Entity SelectedSecondEntity
        {
            get => _selectedSecondEntity;
            set => SetProperty(ref _selectedSecondEntity, value);
        }

        public void HandleDrop(Entity entity, string canvasName)
        {
            if (EntitiesTreeView == null) throw new InvalidOperationException("EntitiesTreeView is not initialized.");
            if (CanvasEntities == null || !CanvasEntities.ContainsKey(canvasName)) throw new InvalidOperationException($"CanvasEntities does not contain {canvasName}.");

            if (CanvasEntities[canvasName].Any())
            {
                return; // Do not proceed with the drop
            }

            foreach (var entityByType in EntitiesTreeView)
            {
                if (entityByType.Entities == null) continue;
                if (entityByType.Entities.Contains(entity))
                {
                    entityByType.Entities.Remove(entity);
                    break;
                }
            }

            foreach (var canvas in CanvasEntities)
            {
                if (canvas.Value.Contains(entity))
                {
                    canvas.Value.Remove(entity);
                    break;
                }
            }

            CanvasEntities[canvasName].Clear();
            CanvasEntities[canvasName].Add(entity);
            if (!EntitiesInCanvas.Contains(entity))
            {
                EntitiesInCanvas.Add(entity);
            }
            var networkDisplayView = new NetworkDisplayView();
            if (networkDisplayView != null)
            {
                var canvas = networkDisplayView.FindName(canvasName) as Canvas;
                if (canvas != null)
                {
                    entity.X = canvas.Margin.Left + canvas.Width / 2;
                    entity.Y = canvas.Margin.Top + canvas.Height / 2;
                }
            }


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
                EntitiesInCanvas.Remove(entity);

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
                var connectionsToRemove = EntityConnections.Where(c => c.Entity1 == entity || c.Entity2 == entity).ToList();
                foreach (var connection in connectionsToRemove)
                {
                    EntityConnections.Remove(connection);
                }
                MainWindowViewModel.ShowToastNotification(new ToastNotification("Success", "Entity cleared out of canvas!", Notification.Wpf.NotificationType.Success));
                OnPropertyChanged(nameof(CanvasEntities));
                OnPropertyChanged(nameof(EntitiesTreeView));
                UpdateCanvasBorderColors();
                
            }
        }

        private bool CanConnectEntities()
        {
            return SelectedFirstEntity != null && SelectedSecondEntity != null;
        }
        private void ConnectEntities()
        {
            if (SelectedFirstEntity == null || SelectedSecondEntity == null || SelectedFirstEntity == SelectedSecondEntity)
            {
                return;
            }

            var connection = new Connection(SelectedFirstEntity, SelectedSecondEntity);
            EntityConnections.Add(connection);
            
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
