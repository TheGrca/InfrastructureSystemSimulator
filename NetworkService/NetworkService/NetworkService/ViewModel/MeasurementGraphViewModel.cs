using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Win32;
using NetworkService.Model;

namespace NetworkService.ViewModel
{
    public class MeasurementGraphViewModel : BindableBase
    {
        public ObservableCollection<Entity> Entities { get; set; }
        public MeasurementGraphViewModel()
        {
            var networkEntitiesViewModel = new NetworkEntitiesViewModel();

            Entities = networkEntitiesViewModel.Entities;
        }

        private string _logFilePath = "log.txt";
        private int _lastReadPosition = 0;
        private List<EntityValue> _entityValues = new List<EntityValue>();

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
                        // Process each line of the log file
                        ProcessLogLine(line);
                    }

                    // Update the last read position
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
                    int index = int.Parse(entityParts[1]);
                    string[] entityParts2 = entityParts[2].Split(':');
                    {
                        double value;
                        if (double.TryParse(entityParts2[2], out value))
                        {
                            _entityValues.Add(new EntityValue { TimeStamp = DateTime.Parse(timeLog), Index = index, Value = value });
                        }
                    }
                }
            }
        }


    }
}
