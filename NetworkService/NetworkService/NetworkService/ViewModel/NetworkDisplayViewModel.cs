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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NetworkService.Model;

namespace NetworkService.ViewModel
{
    public class NetworkDisplayViewModel : BindableBase
    {
        public BindingList<Entity> Entities {  get; set; }
        public ObservableCollection<Brush> Brushes { get; set; }
        public ObservableCollection<Canvas> Canvases { get; set; }
        public ObservableCollection<Model.Line> Lines { get; set; }
        public ObservableCollection<string> Descriptions { get; set; }

        private Entity selectedEntity;
        public Entity SelectedEntity
        {
            get { return selectedEntity; }
            set
            {
                selectedEntity = value;
                OnPropertyChanged("SelectedEntity");
            }
        }
        private Entity draggedItem = null;
        private bool isdragging = false;
        public int draggingSourceIndex = -1;
        private bool isLineSourceSelected = false;
        private int sourceCanvasIndex = -1;
        private int destinationCanvasIndex = -1;
        private Model.Line currentLine = new Model.Line();
        private Point linePoint1 = new Point();
        private Point linePoint2 = new Point();

        public MyICommand<object> DropEntityOnCanvas { get; set; }
        public MyICommand<object> LeftMouseButtonDownOnCanvas { get; set; }
        public MyICommand MouseLeftButtonUp { get; set; }
        public MyICommand<object> SelectionChanged { get; set; }
        public MyICommand<object> FreeCanvas { get; set; }
        public MyICommand<object> RightMouseButtonDownOnCanvas { get; set; }



        public NetworkDisplayViewModel()
        {
            Entities = new BindingList<Entity>();
            Lines = new ObservableCollection<Model.Line>();
            Canvases = new ObservableCollection<Canvas>();
            Brushes = new ObservableCollection<Brush>();
            Descriptions = new ObservableCollection<string>();
            DropEntityOnCanvas = new MyICommand<object>(OnDrop);
            LeftMouseButtonDownOnCanvas = new MyICommand<object>(OnLeftMouseButtonDown);
            MouseLeftButtonUp = new MyICommand(OnMouseLeftButtonUp);
            SelectionChanged = new MyICommand<object>(OnSelectionChanged);
            FreeCanvas = new MyICommand<object>(OnFreeCanvas);
            RightMouseButtonDownOnCanvas = new MyICommand<object>(OnRightMouseButtonDown);

        }

        private void OnLeftMouseButtonDown(object parameter)
        {
            if (!isdragging)
            {
                int index = Convert.ToInt32(parameter);

                if (Canvases[index].Resources["taken"] != null)
                {
                    isdragging = true;
                    draggedItem = (Entity)(Canvases[index].Resources["data"]);
                    draggingSourceIndex = index;
                    DragDrop.DoDragDrop(Canvases[index], draggedItem, DragDropEffects.Move);
                }
            }
        }
        private void OnMouseLeftButtonUp()
        {
            draggedItem = null;
            SelectedEntity = null;
            isdragging = false;
            draggingSourceIndex = -1;
        }
        private void OnSelectionChanged(object parameter)
        {
            if (!isdragging)
            {
                isdragging = true;
                draggedItem = SelectedEntity;
                DragDrop.DoDragDrop((ListView)parameter, draggedItem, DragDropEffects.Move);
            }
        }
        private void OnRightMouseButtonDown(object parameter)
        {
            int index = Convert.ToInt32(parameter);

            if (Canvases[index].Resources["taken"] != null)
            {
                if (!isLineSourceSelected)
                {
                    sourceCanvasIndex = index;

                    linePoint1 = GetPointForCanvasIndex(sourceCanvasIndex);

                    currentLine.X1 = linePoint1.X;
                    currentLine.Y1 = linePoint1.Y;
                    currentLine.Start = sourceCanvasIndex;

                    isLineSourceSelected = true;
                }
                else
                {
                    destinationCanvasIndex = index;

                    if ((sourceCanvasIndex != destinationCanvasIndex) && !DoesLineAlreadyExist(sourceCanvasIndex, destinationCanvasIndex))
                    {
                        linePoint2 = GetPointForCanvasIndex(destinationCanvasIndex);

                        currentLine.X2 = linePoint2.X;
                        currentLine.Y2 = linePoint2.Y;
                        currentLine.End = destinationCanvasIndex;

                        Lines.Add(new Model.Line
                        {
                            X1 = currentLine.X1,
                            Y1 = currentLine.Y1,
                            X2 = currentLine.X2,
                            Y2 = currentLine.Y2,
                            Start = currentLine.Start,
                            End = currentLine.End
                        });

                        isLineSourceSelected = false;

                        linePoint1 = new Point();
                        linePoint2 = new Point();
                        currentLine = new Model.Line();
                    }
                    else
                    {
                        // Pocetak i kraj linije su u istom canvasu

                        isLineSourceSelected = false;

                        linePoint1 = new Point();
                        linePoint2 = new Point();
                        currentLine = new Model.Line();
                    }
                }
            }
            else
            {
                // Canvas na koji se postavlja tacka nije zauzet

                isLineSourceSelected = false;

                linePoint1 = new Point();
                linePoint2 = new Point();
                currentLine = new Model.Line();
            }
        }

        public int GetCanvasIndexForEntityId(int entityId)
        {
            for (int i = 0; i < Canvases.Count; i++)
            {
                Entity entity = (Canvases[i].Resources["data"]) as Entity;

                if ((entity != null) && (entity.Id == entityId))
                {
                    return i;
                }
            }
            return -1;
        }


        private void OnDrop(object parameter)
        {
            if (draggedItem != null)
            {
                int index = Convert.ToInt32(parameter);

                if (Canvases[index].Resources["taken"] == null)
                {
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                   // logo.UriSource = new Uri(draggedItem.EntityType.ImagePath, UriKind.RelativeOrAbsolute);
                    logo.EndInit();

                    Canvases[index].Background = new ImageBrush(logo);
                    Canvases[index].Resources.Add("taken", true);
                    Canvases[index].Resources.Add("data", draggedItem);
                    if(draggedItem.Value > 2.73 || draggedItem.Value < 0.34)
                    {
                        //Brushes[index] = 
                    }
                    else
                    {
                        //Brushes[index] =
                    }
                    Descriptions[index] = $"ID: {draggedItem.Id} Value: {draggedItem.Value}";

                    // PREVLACENJE IZ DRUGOG CANVASA
                    if (draggingSourceIndex != -1)
                    {
                        Canvases[draggingSourceIndex].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#949BA4"));

                        Canvases[draggingSourceIndex].Resources.Remove("taken");
                        Canvases[draggingSourceIndex].Resources.Remove("data");
                        Brushes[draggingSourceIndex] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E1F22"));
                        Descriptions[draggingSourceIndex] = (" ");


                        UpdateLinesForCanvas(draggingSourceIndex, index);

                        // Crtanje linije se prekida ako je, izmedju postavljanja tacaka, entitet pomeren na drugo polje
                        if (sourceCanvasIndex != -1)
                        {
                            isLineSourceSelected = false;
                            sourceCanvasIndex = -1;
                            linePoint1 = new Point();
                            linePoint2 = new Point();
                            currentLine = new Model.Line();
                        }

                        draggingSourceIndex = -1;
                    }

                    // PREVLACENJE IZ LISTE
                    if (Entities.Contains(draggedItem))
                    {
                        Entities.Remove(draggedItem);
                    }
                }
            }
        }

        public void UpdateEntityOnCanvas(Entity entity)
        {
            int canvasIndex = GetCanvasIndexForEntityId(entity.Id);

            if (canvasIndex != -1)
            {
                Descriptions[canvasIndex] = ($"ID: {entity.Id} Value: {entity.Value}");
                if (entity.Value > 2.73 || entity.Value < 0.34)
                {
//Brushes[canvasIndex] = Brushes.Green;
                }
                else
                {
                   // Brushes[canvasIndex] = Brushes.Red;
                }
            }
        }

        private void UpdateLinesForCanvas(int sourceCanvas, int destinationCanvas)
        {
            for (int i = 0; i < Lines.Count; i++)
            {
                if (Lines[i].Start == sourceCanvas)
                {
                    Point newSourcePoint = GetPointForCanvasIndex(destinationCanvas);
                    Lines[i].X1 = newSourcePoint.X;
                    Lines[i].Y1 = newSourcePoint.Y;
                    Lines[i].Start = destinationCanvas;
                }
                else if (Lines[i].End == sourceCanvas)
                {
                    Point newDestinationPoint = GetPointForCanvasIndex(destinationCanvas);
                    Lines[i].X2 = newDestinationPoint.X;
                    Lines[i].Y2 = newDestinationPoint.Y;
                    Lines[i].End = destinationCanvas;
                }
            }
        }

        public void DeleteEntityFromCanvas(Entity entity)
        {
            int canvasIndex = GetCanvasIndexForEntityId(entity.Id);

            if (canvasIndex != -1)
            {
                Canvases[canvasIndex].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#949BA4"));
                Canvases[canvasIndex].Resources.Remove("taken");
                Canvases[canvasIndex].Resources.Remove("data");
                Brushes[canvasIndex] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E1F22"));
                Descriptions[canvasIndex] = ($" ");

                DeleteLinesForCanvas(canvasIndex);
            }
        }
        private void DeleteLinesForCanvas(int canvasIndex)
        {
            List<Model.Line> linesToDelete = new List<Model.Line>();

            for (int i = 0; i < Lines.Count; i++)
            {
                if ((Lines[i].Start == canvasIndex) || (Lines[i].End == canvasIndex))
                {
                    linesToDelete.Add(Lines[i]);
                }
            }

            foreach (Model.Line line in linesToDelete)
            {
                Lines.Remove(line);
            }
        }
        private void OnFreeCanvas(object parameter)
        {
            int index = Convert.ToInt32(parameter);

            if (Canvases[index].Resources["taken"] != null)
            {
                // Crtanje linije se prekida ako je, izmedju postavljanja tacaka, entitet uklonjen sa canvas-a
                if (sourceCanvasIndex != -1)
                {
                    isLineSourceSelected = false;
                    sourceCanvasIndex = -1;
                    linePoint1 = new Point();
                    linePoint2 = new Point();
                    currentLine = new Model.Line();
                }

                DeleteLinesForCanvas(index);

                Entities.Add((Entity)Canvases[index].Resources["data"]);
                Canvases[index].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#949BA4"));
                Canvases[index].Resources.Remove("taken");
                Canvases[index].Resources.Remove("data");
                Brushes[index] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E1F22"));
                Descriptions[index] = ($" ");
            }
        }

        private bool DoesLineAlreadyExist(int start, int end)
        {
            foreach (Model.Line line in Lines)
            {
                if ((line.Start == start) && (line.End == end))
                {
                    return true;
                }
                if ((line.Start == end) && (line.End == start))
                {
                    return true;
                }
            }
            return false;
        }
        // Centralna tacka na Canvas kontroli
        private Point GetPointForCanvasIndex(int canvasIndex)
        {
            double x = 0, y = 0;

            for (int row = 0; row <= 3; row++)
            {
                for (int col = 0; col <= 2; col++)
                {
                    int currentIndex = row * 3 + col;

                    if (canvasIndex == currentIndex)
                    {
                        x = 50 + (col * 135);
                        y = 50 + (row * 135);

                        break;
                    }
                }
            }
            return new Point(x, y);
        }
    }
}
