using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Win32;
using NetworkService.Model;

namespace NetworkService.ViewModel
{
    public class MeasurementGraphViewModel : BindableBase
    {
        public MeasurementGraphViewModel()
        {
            StartReadingLogFile();
            _selectionHistory = new Stack<Entity>();
            UndoSelectionCommand = new MyICommand(OnUndoSelection, CanUndoSelection);
        }

        private string _logFilePath = "log.txt";
        private int _lastReadPosition = 0;
        private Dictionary<int, List<EntityValue>> _latestValues = new Dictionary<int, List<EntityValue>>();
        private Dictionary<int, Thickness> _ellipseMargins = new Dictionary<int, Thickness>();
        private Dictionary<int, Brush> _ellipseStrokeColors = new Dictionary<int, Brush>();
        private Dictionary<int, Point> _ellipseConnectionPoints = new Dictionary<int, Point>();
        private Dictionary<int, string> _latestValuesTime = new Dictionary<int, string>();
        private List<EntityValue> _selectedEntityValues = new List<EntityValue>();

        public List<EntityValue> SelectedEntityValues
        {
            get 
            {
                return _selectedEntityValues; 
            }
            set
            { 
                if(_selectedEntityValues != value)
                {
                    _selectedEntityValues = value;
                    OnPropertyChanged(nameof(SelectedEntityValues));
                }
            }
        }

        public Dictionary<int, Brush> EllipseStrokeColors
        {
            get { return _ellipseStrokeColors; }
        }
        public Dictionary<int, string> LatestValuesTime
        {
            get { return _latestValuesTime; }
        }
        public Dictionary<int, Point> ElipseConnectionPoint
        {
            get { return _ellipseConnectionPoints; }
        }



        public Dictionary<int, double> LastValues
        {
            get
            {
                return _latestValues.ToDictionary(kv => kv.Key, kv => kv.Value.LastOrDefault()?.Value ?? 0.0);
            }
        }


        public void StartReadingLogFile()
        {
            System.Timers.Timer timer = new System.Timers.Timer(1000);
            timer.Elapsed += TimerElapsed;
            timer.AutoReset = true;
            timer.Start();
        }

        public void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            using (FileStream fs = new FileStream(_logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fs.Seek(_lastReadPosition, SeekOrigin.Begin);
                using (StreamReader sr = new StreamReader(fs))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        ProcessLogLine(line);
                    }
                    _lastReadPosition = (int)fs.Position;
                }
            }
        }

        private void ProcessLogLine(string line)
        {
            string[] parts = line.Split('|');
            if (parts.Length == 2)
            {
                string timeLog = parts[0];
                string[] entityParts = parts[1].Split('_');
                if(entityParts.Length == 2)
                {
                    string[] entityParts2 = entityParts[1].Split(':');
                    {
                        int index = int.Parse(entityParts2[0]);
                        double value;
                        if (double.TryParse(entityParts2[1], out value))
                        {
                            var entityValue = new EntityValue { TimeStamp = DateTime.Parse(timeLog), Index = index, Value = value };
                            AddLatestValue(entityValue);
                           
                        }
                    }
                }
            }
        }

        private void AddLatestValue(EntityValue entityValue)
        {
            if (!_latestValues.ContainsKey(entityValue.Index))
            {
                _latestValues[entityValue.Index] = new List<EntityValue>();
                for (int i = 0; i < 5; i++)
                {
                    _latestValues[entityValue.Index].Add(new EntityValue { Value = 0, TimeStamp = DateTime.MinValue });
                }
            }

            var latestValues = _latestValues[entityValue.Index];
            latestValues.Add(entityValue);

            // Keep only the latest 5 values
            if (latestValues.Count > 5)
            {
                latestValues.RemoveAt(0);
            }

            // Update ellipse margins for the selected entity
            if (entityValue.Index == SelectedIndex)
            {
                Application.Current.Dispatcher.Invoke(() => UpdateEllipsePropertiesForSelectedEntity());
            }

            // Notify UI that the latest values for this entity index have changed
            Application.Current.Dispatcher.Invoke(() => {
                OnPropertyChanged($"LatestValues_{entityValue.Index}");
            OnPropertyChanged(nameof(LastValues));
            OnPropertyChanged(nameof(SelectedEntityValues));
        });
        }

        // Method to get the latest 5 values for a given entity index
        public List<EntityValue> GetLatestValues(int index)
        {
            if (_latestValues.ContainsKey(index))
            {
                return _latestValues[index];
            }
            else
            {
                return new List<EntityValue>();
            }
        }

        private Thickness CalculateProportionalMargin(double value)
        {
            double maxMargin = 250; // Maximum margin value
            double minMargin = -125; // Minimum margin value
            double proportionalMargin = (value / 5.50) * (maxMargin - minMargin) + minMargin;
            return new Thickness(0, 0, 0, proportionalMargin);
        }

        private void UpdateEllipsePropertiesForSelectedEntity()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // Clear existing margins
                _ellipseMargins.Clear();
                _ellipseStrokeColors.Clear();
                _latestValuesTime.Clear();

                // Get the latest values for the selected entity
                var selectedEntityIndex = SelectedIndex;
                var latestValuesForSelectedEntity = GetLatestValues(selectedEntityIndex);

                // If the number of values is less than 5, fill the remaining ellipses with a value of 0
                while (latestValuesForSelectedEntity.Count < 5)
                {
                    latestValuesForSelectedEntity.Insert(0, new EntityValue { Value = 0, TimeStamp = DateTime.MinValue }); // Insert 0 at the beginning
                }
                var newPolylinePoints = new PointCollection();

                // Calculate proportional margins for the latest values of the selected entity
                for (int i = 0; i < latestValuesForSelectedEntity.Count; i++)
                {
                    double latestValue = Math.Round(latestValuesForSelectedEntity[i].Value, 2);
                    latestValuesForSelectedEntity[i].Value = latestValue;
                    _ellipseMargins[i] = CalculateProportionalMargin(latestValue);
                    _ellipseStrokeColors[i] = latestValue < 0.34 || latestValue > 2.73 ? Brushes.Red : Brushes.Green;
                    string time = latestValuesForSelectedEntity[i].TimeStamp.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
                    _latestValuesTime[i] = time;
                    newPolylinePoints.Add(new Point(i * 75, CalculateYValue(latestValue)));
                }
                PolyLinePoints = newPolylinePoints;

                SelectedEntityValues = latestValuesForSelectedEntity;
                OnPropertyChanged(nameof(PolyLinePoints));
                OnPropertyChanged(nameof(EllipseMargins));
                OnPropertyChanged(nameof(EllipseStrokeColors));
                OnPropertyChanged(nameof(LatestValuesTime));

            });
        }

        // Property to bind ellipse margins to in XAML
        public Dictionary<int, Thickness> EllipseMargins
        {
            get { return _ellipseMargins; }
        }


        private int _selectedIndex;
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                if (_selectedIndex != value)
                {
                    _selectedIndex = value;
                    UpdateEllipsePropertiesForSelectedEntity();
                    OnPropertyChanged(nameof(SelectedIndex));
                }
            }
        }


        //Ellipse lines

        private int CalculateYValue(double entityValue)
        {
            double maxY = 10; 
            double minY = 225; 

            return (int)Math.Round(((entityValue - 0.01) / (5.50 - 0.01)) * (maxY - minY) + minY);
        }

        private PointCollection _polylinePoints = new PointCollection();
        
        public PointCollection PolyLinePoints
        {
            get
            {
                return _polylinePoints;
            }
            set
            {
                    _polylinePoints = value;
                    OnPropertyChanged(nameof(PolyLinePoints));
            }
        }

        //UNDO
        private bool _isUndoSelectionButtonEnabled;
        private Entity _selectedComboBoxEntity;
        private Stack<Entity> _selectionHistory;
        public bool IsUndoSelectionButtonEnabled
        {
            get { return _isUndoSelectionButtonEnabled; }
            set
            {
                if (_isUndoSelectionButtonEnabled != value)
                {
                    _isUndoSelectionButtonEnabled = value;
                    OnPropertyChanged(nameof(IsUndoSelectionButtonEnabled));
                }
            }
        }

        public Entity SelectedComboBoxEntity
        {
            get { return _selectedComboBoxEntity; }
            set
            {
                if (_selectedComboBoxEntity != value)
                {
                    if (_selectedComboBoxEntity != null)
                    {
                        _selectionHistory.Push(_selectedComboBoxEntity); // Save current selection before changing
                    }
                    _selectedComboBoxEntity = value;
                    IsUndoSelectionButtonEnabled = _selectionHistory.Count > 0; // Enable the undo button if there's history
                    UndoSelectionCommand.RaiseCanExecuteChanged();
                    OnPropertyChanged(nameof(SelectedComboBoxEntity));
                }
            }
        }

        public MyICommand UndoSelectionCommand { get; private set; }

        private void OnUndoSelection()
        {
            if (_selectionHistory.Count > 0)
            {
                var previousSelection = _selectionHistory.Pop();
                _selectedComboBoxEntity = previousSelection;
                OnPropertyChanged(nameof(SelectedComboBoxEntity));
                IsUndoSelectionButtonEnabled = _selectionHistory.Count > 0;
                UndoSelectionCommand.RaiseCanExecuteChanged();
            }
        }

        private bool CanUndoSelection()
        {
            return _selectionHistory.Count > 0;
        }
    }
}
