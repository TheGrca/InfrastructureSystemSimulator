using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NetworkService.Model;

namespace NetworkService.ViewModel
{
    public class NetworkDisplayViewModel : BindableBase
    {
        public ICommand ClearCanvasCommand { get; }
        public ICommand StartDragCommand { get; }
        public ICommand EndDragCommand { get; }


        public NetworkDisplayViewModel()
        {
            EntitiesTreeView = MainWindowViewModel.EntitiesTreeView;

            CanvasEntities = new Dictionary<string, ObservableCollection<Entity>>();

            for (int i = 1; i <= 12; i++)
            {
                CanvasEntities.Add($"Canvas{i}", new ObservableCollection<Entity>());
            }
            ClearCanvasCommand = new MyICommand<string>(ClearCanvas);
            StartDragCommand = new MyICommand<Entity>(StartDrag);
            EndDragCommand = new MyICommand<Entity>(EndDrag);
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

                OnPropertyChanged(nameof(CanvasEntities));
                OnPropertyChanged(nameof(EntitiesTreeView));
            }
        }

        public void HandleCanvasDrop(Entity entity, string sourceCanvasName, string targetCanvasName)
        {
            if (EntitiesTreeView == null) throw new InvalidOperationException("EntitiesTreeView is not initialized.");
            if (CanvasEntities == null || !CanvasEntities.ContainsKey(sourceCanvasName) || !CanvasEntities.ContainsKey(targetCanvasName))
                throw new InvalidOperationException($"CanvasEntities does not contain {sourceCanvasName} or {targetCanvasName}.");

            // Remove the entity from the source canvas
            var sourceEntities = CanvasEntities[sourceCanvasName];
            sourceEntities.Remove(entity);

            // Add the entity to the target canvas
            var targetEntities = CanvasEntities[targetCanvasName];
            targetEntities.Add(entity);

            OnPropertyChanged(nameof(EntitiesTreeView));
            OnPropertyChanged(nameof(CanvasEntities));
        }

        private Entity _draggedEntity;

        private void StartDrag(Entity entity)
        {
            _draggedEntity = entity;
        }

        private void EndDrag(Entity entity)
        {
            if (_draggedEntity != null)
            {
                // Find the canvas under the mouse pointer
                var canvasUnderMouse = FindCanvasUnderMouse();
                if (canvasUnderMouse != null)
                {
                    HandleCanvasDrop(_draggedEntity, _draggedEntity.CurrentCanvas, canvasUnderMouse.Name);
                    _draggedEntity.CurrentCanvas = canvasUnderMouse.Name;
                }
                _draggedEntity = null;
            }
        }

        private Canvas FindCanvasUnderMouse()
        {
            var mousePosition = Mouse.GetPosition((IInputElement)Application.Current.MainWindow.Content);
            var canvasUnderMouse = VisualTreeHelper.HitTest((Visual)Application.Current.MainWindow.Content, mousePosition)?.VisualHit;
            while (canvasUnderMouse != null && !(canvasUnderMouse is Canvas))
            {
                canvasUnderMouse = VisualTreeHelper.GetParent(canvasUnderMouse);
            }
            return (Canvas)canvasUnderMouse;
        }
    }
}
