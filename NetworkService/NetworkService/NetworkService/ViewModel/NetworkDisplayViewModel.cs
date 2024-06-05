using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Linq;
using System.Diagnostics;
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
using Microsoft.TeamFoundation.SourceControl.WebApi.Legacy;
using NetworkService.Model;
using NetworkService.Views;
using Notifications.Wpf.ViewModels.Base;

namespace NetworkService.ViewModel
{

    public class NetworkDisplayViewModel : BindableBase
    {
        private readonly DispatcherTimer _timer;
        public ICommand ClearCanvasCommand { get; }


        public NetworkDisplayViewModel()
        {
            EntitiesTreeView = MainWindowViewModel.EntitiesTreeView;
            CanvasEntities = MainWindowViewModel.CanvasEntities;
            EntityConnections = MainWindowViewModel.EntityConnections;

            ClearCanvasCommand = new MyICommand<string>(ClearCanvas);
            ConnectCommand = new MyICommand<object>(ConnectEntities);
            UndoCanvasCommand = new MyICommand(OnUndoCanvas);
            UpdateCanUndoCanvas();

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(0.001)
            };
            _timer.Tick += (sender, args) => UpdateCanvasBorderColors();
            _timer.Tick += (sender, args) => UpdateConnectedEntities();
            _timer.Start();
            EntitiesInCanvas = MainWindowViewModel.EntitiesInCanvas;
            EntitiesInCanvas.CollectionChanged += EntitiesInCanvas_CollectionChanged;
        }
        

        public Dictionary<string, ObservableCollection<Model.Entity>> CanvasEntities { get; set; }
        public ObservableCollection<EntityByType> EntitiesTreeView { get; set; }
        public ObservableCollection<Model.Entity> EntitiesInCanvas { get; set; }
        public ObservableCollection<Connection> EntityConnections { get; set; }

        public void HandleDrop(Model.Entity entity, string canvasName)
        {

            string previousCanvasName = null;

            // Check if the entity is already in any canvas
            foreach (var canvas in CanvasEntities)
            {
                if (canvas.Value.Contains(entity))
                {
                    previousCanvasName = canvas.Key;
                    break;
                }
            }

            // If the entity is moving to a different canvas
            if (previousCanvasName != null && previousCanvasName != canvasName)
            {
                CanvasEntities[previousCanvasName].Remove(entity);
                CanvasEntities[canvasName].Clear();
                CanvasEntities[canvasName].Add(entity);

                // Add to undo history
                AddToCanvasHistory(new Stack<object>(new object[] { entity, previousCanvasName }), 2);
            }
            else if (previousCanvasName == null) // Entity is moving from tree view to a canvas
            {
                foreach (var entityByType in EntitiesTreeView)
                {
                    if (entityByType.Entities == null) continue;
                    if (entityByType.Entities.Contains(entity))
                    {
                        entityByType.Entities.Remove(entity); // Remove from the tree view
                        break;
                    }
                }

                CanvasEntities[canvasName].Clear();
                CanvasEntities[canvasName].Add(entity);
                if (!EntitiesInCanvas.Contains(entity))
                {
                    EntitiesInCanvas.Add(entity);
                }

                // Add to undo history
                AddToCanvasHistory(new Stack<object>(new object[] { entity }), 1);
            }

            OnRemoveAllLinesRequested(EventArgs.Empty);
            UpdateConnectedEntities();

            OnPropertyChanged(nameof(EntitiesTreeView));
            OnPropertyChanged(nameof(CanvasEntities));
            OnPropertyChanged(nameof(EntityConnections));
            OnPropertyChanged(nameof(EntitiesInCanvas));
            UpdateCanvasBorderColors();
        }

        public void ClearCanvas(string canvasName)
        {
            if (CanvasEntities.TryGetValue(canvasName, out var entities) && entities.Any())
            {
                var entity = entities.First();
                entities.Clear();
                EntitiesInCanvas.Remove(entity);

               
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
                        Entities = new ObservableCollection<Model.Entity> { entity }
                    });
                }

                var connectionsToRemove = EntityConnections.Where(c => c.Entity1 == entity || c.Entity2 == entity).ToList();
                foreach (var connection in connectionsToRemove)
                {
                    EntityConnections.Remove(connection);
                }

                AddToCanvasHistory(new Stack<object>(new object[] { entity, canvasName }), 3);


                OnRemoveAllLinesRequested(EventArgs.Empty);
                UpdateConnectedEntities();
                MainWindowViewModel.ShowToastNotification(new ToastNotification("Success", "Entity cleared out of canvas!", Notification.Wpf.NotificationType.Success));
                OnPropertyChanged(nameof(CanvasEntities));
                OnPropertyChanged(nameof(EntitiesTreeView));
                OnPropertyChanged(nameof(EntitiesInCanvas));
                UpdateCanvasBorderColors();
            }
            else
            {
                MainWindowViewModel.ShowToastNotification(new ToastNotification("Error", "Cannot clear empty canvas!", Notification.Wpf.NotificationType.Error));
            }
        }





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

       

        //LINES

        private Model.Entity _selectedEntity1;
        private Model.Entity _selectedEntity2;
        public Model.Entity SelectedEntity1
        {
            get { return _selectedEntity1; }
            set
            {
                _selectedEntity1 = value;
                OnPropertyChanged(nameof(SelectedEntity1));
            }
        }

        public Model.Entity SelectedEntity2
        {
            get { return _selectedEntity2; }
            set
            {
                _selectedEntity2 = value;
                OnPropertyChanged(nameof(SelectedEntity2));
            }
        }
        public event EventHandler<Tuple<Point, Point>> LineDrawRequested;
        public event EventHandler RemoveAllLinesRequested;
        public ICommand ConnectCommand { get; }
        private void ConnectEntities(object parameter)
        {
            if (SelectedEntity1 != null && SelectedEntity2 != null)
            {
                if (SelectedEntity1 == SelectedEntity2)
                {
                    MainWindowViewModel.ShowToastNotification(new ToastNotification("Error", "Cannot connect the same entity to itself!", Notification.Wpf.NotificationType.Error));
                    SelectedEntity1 = null;
                    SelectedEntity2 = null;
                    return;
                }


                bool connectionExists = EntityConnections.Any(connection =>
                (connection.Entity1 == SelectedEntity1 && connection.Entity2 == SelectedEntity2) ||
                (connection.Entity1 == SelectedEntity2 && connection.Entity2 == SelectedEntity1));

                if (connectionExists)
                {
                    MainWindowViewModel.ShowToastNotification(new ToastNotification("Error", "Connection between these entities already exists!", Notification.Wpf.NotificationType.Error));
                    SelectedEntity1 = null;
                    SelectedEntity2 = null;
                    return;
                }

                if (EntitiesInCanvas.Contains(SelectedEntity1) && EntitiesInCanvas.Contains(SelectedEntity2))
                {
                    EntityConnections.Add(new Connection(SelectedEntity1, SelectedEntity2));
                    var canvasNames = GetCanvasNamesContainingEntities(SelectedEntity1, SelectedEntity2);

                     if (canvasCoordinates.ContainsKey(canvasNames.Item1) && canvasCoordinates.ContainsKey(canvasNames.Item2))
                         {
                            var position1 = canvasCoordinates[canvasNames.Item1];
                            var position2 = canvasCoordinates[canvasNames.Item2];
                            LineDrawRequested?.Invoke(this, Tuple.Create(position1, position2));
                         }
                    AddToCanvasHistory(new Stack<object>(new object[] { SelectedEntity1, SelectedEntity2 }), 4);
                }

                SelectedEntity1 = null;
                SelectedEntity2 = null;
            }
            else
            {
                MainWindowViewModel.ShowToastNotification(new ToastNotification("Error", "You need to select both entities for connection!", Notification.Wpf.NotificationType.Error));
            }
        }

        public void UpdateConnectedEntities()
        {
            foreach(var pair in EntityConnections)
            {
                var canvasNames = GetCanvasNamesContainingEntities(pair.Entity1, pair.Entity2);
                if (canvasCoordinates.ContainsKey(canvasNames.Item1) && canvasCoordinates.ContainsKey(canvasNames.Item2))
                {
                    var position1 = canvasCoordinates[canvasNames.Item1];
                    var position2 = canvasCoordinates[canvasNames.Item2];
                    LineDrawRequested?.Invoke(this, Tuple.Create(position1, position2));
                }
            }
        }

        public Tuple<string, string> GetCanvasNamesContainingEntities(Model.Entity entity1, Model.Entity entity2 = null)
        {
            string entity1Canvas = "";
            string entity2Canvas = "";

            foreach (var kvp in CanvasEntities)
            {
                if (kvp.Value.Contains(entity1))
                {
                    entity1Canvas = kvp.Key;
                }

                if (kvp.Value.Contains(entity2))
                {
                    entity2Canvas = kvp.Key;
                }
            }

            return new Tuple<string, string>(entity1Canvas, entity2Canvas);
        }

        private readonly Dictionary<string, Point> canvasCoordinates = new Dictionary<string, Point>
        {
            { "Canvas1", new Point(30, 370) },
            { "Canvas2", new Point(95, 370) },
            { "Canvas3", new Point(160, 370) },
            { "Canvas4", new Point(225, 370) },
            { "Canvas5", new Point(290, 370) },
            { "Canvas6", new Point(355, 370) },
            { "Canvas7", new Point(30, 500) },
            { "Canvas8", new Point(95, 500) },
            { "Canvas9", new Point(160, 500) },
            { "Canvas10", new Point(225, 500) },
            { "Canvas11", new Point(290, 500) },
            { "Canvas12", new Point(355, 500) }
        };

        protected virtual void OnRemoveAllLinesRequested(EventArgs e)
        {
            RemoveAllLinesRequested?.Invoke(this, e);
        }

        private void EntitiesInCanvas_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(IsConnectEnabled));
        }

        public bool IsConnectEnabled => EntitiesInCanvas.Count >= 2;


        //UNDO
        /*
        DropFromTreeViewToCanvas = 1,
        DropBetweenCanvases = 2,
        DrawLine = 3,
        ClearCanvas = 4,
         */
        private readonly Stack<Tuple<Stack<object>, int>> _canvasHistory = new Stack<Tuple<Stack<object>, int>>();
        public MyICommand UndoCanvasCommand { get; }
        public void AddToCanvasHistory(Stack<object> stack, int actionNumber)
        {
            _canvasHistory.Push(Tuple.Create(stack, actionNumber));
            UpdateCanUndoCanvas();
        }

        // Method to undo the last canvas action
        private void OnUndoCanvas()
        {
            if (_canvasHistory.Count > 0)
            {
                var lastAction = _canvasHistory.Pop();
                var lastActionNumber = lastAction.Item2;
                var dataStack = lastAction.Item1;

                if (lastActionNumber == 1)
                {
                    var entity = (Model.Entity)dataStack.Pop();
                    var canvasName = CanvasEntities.FirstOrDefault(c => c.Value.Contains(entity)).Key;

                    if (canvasName != null)
                    {
                        CanvasEntities[canvasName].Remove(entity);
                        EntitiesInCanvas.Remove(entity);
                        var entityType = EntitiesTreeView.FirstOrDefault(e => e.Type == entity.EntityType.ToString());
                        if (entityType != null)
                        {
                            entityType.Entities.Add(entity);
                        }
                    }
                }
                else if (lastActionNumber == 2)
                {
                    var previousCanvasName = (string)dataStack.Pop();
                    var entity = (Model.Entity)dataStack.Pop();
                    var currentCanvasName = CanvasEntities.FirstOrDefault(c => c.Value.Contains(entity)).Key;

                    if (currentCanvasName != null)
                    {
                        CanvasEntities[currentCanvasName].Remove(entity);
                        CanvasEntities[previousCanvasName].Add(entity);
                    }
                }
                else if (lastActionNumber == 3)
                {
                    var entity = (Model.Entity)dataStack.Pop();
                    var canvasName = (string)dataStack.Pop();

                    var entityType = EntitiesTreeView.FirstOrDefault(e => e.Type == entity.EntityType.ToString());
                    if (entityType != null)
                    {
                        entityType.Entities.Remove(entity);
                        CanvasEntities[canvasName].Add(entity);
                        if (!EntitiesInCanvas.Contains(entity))
                        {
                            EntitiesInCanvas.Add(entity);
                        }
                    }
                }
                else if (lastActionNumber == 4)
                {
                    var entity1 = (Model.Entity)dataStack.Pop();
                    var entity2 = (Model.Entity)dataStack.Pop();
                    var connection = EntityConnections.FirstOrDefault(c =>
                        (c.Entity1 == entity1 && c.Entity2 == entity2) ||
                        (c.Entity1 == entity2 && c.Entity2 == entity1));

                    if (connection != null)
                    {
                        EntityConnections.Remove(connection);
                    }
                }

                UpdateConnectedEntities();
                OnRemoveAllLinesRequested(EventArgs.Empty);
                OnPropertyChanged(nameof(CanvasEntities));
                OnPropertyChanged(nameof(EntitiesTreeView));
                OnPropertyChanged(nameof(EntityConnections));
                OnPropertyChanged(nameof(EntitiesInCanvas));
                UpdateCanvasBorderColors();
                UpdateCanUndoCanvas();
            }
        }

        private bool _canUndoCanvas;
        public bool CanUndoCanvas
        {
            get { return _canUndoCanvas; }
            set
            {
                if (_canUndoCanvas != value)
                {
                    _canUndoCanvas = value;
                    OnPropertyChanged(nameof(CanUndoCanvas));
                    UndoCanvasCommand.RaiseCanExecuteChanged();
                }
            }
        }
        private void UpdateCanUndoCanvas()
        {
            CanUndoCanvas = _canvasHistory.Count > 0;
        }
    }
}
