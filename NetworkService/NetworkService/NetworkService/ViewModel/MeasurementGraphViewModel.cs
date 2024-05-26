﻿using System;
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
        }

        private string _logFilePath = "log.txt";
        private int _lastReadPosition = 0;
        private Dictionary<int, List<EntityValue>> _latestValues = new Dictionary<int, List<EntityValue>>();
        private Dictionary<int, Thickness> _ellipseMargins = new Dictionary<int, Thickness>();
        private Dictionary<int, Brush> _ellipseStrokeColors = new Dictionary<int, Brush>();
        private Dictionary<int, string> _latestValuesTime = new Dictionary<int, string>();
        private List<EntityValue> _selectedEntityValues = new List<EntityValue>();
        private PointCollection _ellipseConnectionPoints = new PointCollection();
        public PointCollection EllipseConnectionPoints
        {
            get
            {
                return _ellipseConnectionPoints;
            }
            set
            {
                if(_ellipseConnectionPoints != value)
                {
                    _ellipseConnectionPoints = value;
                    OnPropertyChanged(nameof(EllipseConnectionPoints));
                }
            }
        }
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
                           // _entityValues.Add(new EntityValue { TimeStamp = DateTime.Parse(timeLog), Index = index, Value = value });
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
                    latestValuesForSelectedEntity.Add(new EntityValue { Value = 0 }); // Insert 0 at the beginning
                }

                // Calculate proportional margins for the latest values of the selected entity
                for (int i = 0; i < latestValuesForSelectedEntity.Count; i++)
                {
                    double latestValue = Math.Round(latestValuesForSelectedEntity[i].Value, 2);
                    latestValuesForSelectedEntity[i].Value = latestValue;
                    _ellipseMargins[i] = CalculateProportionalMargin(latestValue);
                    _ellipseStrokeColors[i] = latestValue < 0.34 || latestValue > 2.73 ? Brushes.Red : Brushes.Green;
                    string time = latestValuesForSelectedEntity[i].TimeStamp.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
                    _latestValuesTime[i] = time;

                }
                UpdatePolylinePoints(latestValuesForSelectedEntity.Select(ev => ev.Value).ToList());
                SelectedEntityValues = latestValuesForSelectedEntity;

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
        private List<Point> _polylinePoints = new List<Point>();

        public List<Point> PolylinePoints
        {
            get { return _polylinePoints; }
            set
            {
                _polylinePoints = value;
                OnPropertyChanged(nameof(PolylinePoints));
            }
        }

        public void UpdatePolylinePoints(List<double> values)
        {
            PolylinePoints.Clear();

            // Calculate Y coordinates based on the values
            for (int i = 0; i < values.Count; i++)
            {
                double value = values[i];
                // Calculate Y coordinate based on the value and your scaling logic
                double y = CalculateYCoordinate(value);
                // Add the point to the Polyline's points
                PolylinePoints.Add(new Point((i + 1) * 75, y)); // Assuming X coordinates increment by 75
            }

            OnPropertyChanged(nameof(PolylinePoints));
        }

        // Method to calculate Y coordinate based on value
        private double CalculateYCoordinate(double value)
        {
            // Your scaling logic to convert value to Y coordinate
            // Example scaling logic:
            double yMin = 0; // Minimum Y value
            double yMax = 240; // Maximum Y value
            double valueMin = 0.01; // Minimum value from the metering simulator
            double valueMax = 5.50; // Maximum value from the metering simulator
            double y = (value - valueMin) / (valueMax - valueMin) * (yMax - yMin) + yMin;
            return y;
        }

        // Other ViewModel code...
    

}
}
