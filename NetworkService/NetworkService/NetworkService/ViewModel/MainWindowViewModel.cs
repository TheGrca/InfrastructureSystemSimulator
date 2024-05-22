using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetworkService.Model;

namespace NetworkService.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        public static ObservableCollection<Entity> Entities { get; set; }
        public void LoadData()
        {
            Entities = new ObservableCollection<Entity>
            {
                new Entity
                {
                    Id = 4,
                    Name = "Naziv",
                    ImagePath = @"\Resources\Pictures\1.jpg",
                    EntityType = EntityType.IntervalMeter,
                    Value = 0
                },
                new Entity
                {
                    Id = 156,
                    Name = "Naziv2",
                    ImagePath = @"\Resources\Pictures\2.jpg",
                    EntityType = EntityType.IntervalMeter,
                    Value = 0
                },
                new Entity
                {
                    Id = 8,
                    Name = "Naziv3",
                    ImagePath = @"\Resources\Pictures\3.jpg",
                    EntityType = EntityType.IntervalMeter,
                    Value = 0
                },
                new Entity
                {
                    Id = 3,
                    Name = "Nazi4",
                    ImagePath = @"\Resources\Pictures\4.png",
                    EntityType = EntityType.SmartMeter,
                    Value = 0
                }
            };
        }
        private int count = 4; // Inicijalna vrednost broja objekata u sistemu
                                // ######### ZAMENITI stvarnim brojem elemenata
                                //           zavisno od broja entiteta u listi

        public MainWindowViewModel()
        {
            LoadData();
            createListener(); //Povezivanje sa serverskom aplikacijom   
            NavCommand = new MyICommand<string>(OnNav);
            CurrentViewModel = networkEntitiesViewModel;
            history = new Stack<object>();
        }
        private void createListener()
        {
            var tcp = new TcpListener(IPAddress.Any, 25675);
            tcp.Start();

            var listeningThread = new Thread(() =>
            {
                while (true)
                {
                    var tcpClient = tcp.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(param =>
                    {
                        //Prijem poruke
                        NetworkStream stream = tcpClient.GetStream();
                        string incomming;
                        byte[] bytes = new byte[1024];
                        int i = stream.Read(bytes, 0, bytes.Length);
                        //Primljena poruka je sacuvana u incomming stringu
                        incomming = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                        //Ukoliko je primljena poruka pitanje koliko objekata ima u sistemu -> odgovor
                        if (incomming.Equals("Need object count"))
                        {
                            //Response
                            /* Umesto sto se ovde salje count.ToString(), potrebno je poslati 
                             * duzinu liste koja sadrzi sve objekte pod monitoringom, odnosno
                             * njihov ukupan broj (NE BROJATI OD NULE, VEC POSLATI UKUPAN BROJ)
                             * */
                            Byte[] data = System.Text.Encoding.ASCII.GetBytes(Entities.Count.ToString());
                            stream.Write(data, 0, data.Length);
                        }
                        else
                        {
                            //U suprotnom, server je poslao promenu stanja nekog objekta u sistemu
                            Console.WriteLine(incomming); //Na primer: "Entitet_1:272"
                            string[] parts = incomming.Split(':');
                            string fileWrite = $"{DateTime.Now}|{incomming}";
                            WriteFile(fileWrite);
                            UpdateEntityCollection(parts);
                            //################ IMPLEMENTACIJA ####################
                            // Obraditi poruku kako bi se dobile informacije o izmeni
                            // Azuriranje potrebnih stvari u aplikaciji

                        }
                    }, null);
                }
            });

            listeningThread.IsBackground = true;
            listeningThread.Start();
        }

        private void UpdateEntityCollection(string[] parts)
        {
            int Index = Int32.Parse(parts[0].Split('_')[1]);
            double Value = double.Parse(parts[1]);
            Entities[Index].Value = Value;
            CheckValue(Entities[Index]);
        }

        private void CheckValue(Entity entity)
        {
            if (entity.Value < 0.34 || entity.Value > 2.73)
            {
                entity.IsValid = false;
            }
            else
            {
                entity.IsValid = true;
            }
        }

        private void WriteFile(string fileWrite)
        {
            string filepath = "log.txt";
            using (StreamWriter writer = new StreamWriter(filepath, true)) {
                writer.WriteLine(fileWrite);
            }
        }



        /////////////////////////////////////////
        private NetworkEntitiesViewModel networkEntitiesViewModel = new NetworkEntitiesViewModel();
        private NetworkDisplayViewModel networkDisplayViewModel = new NetworkDisplayViewModel();
        private MeasurementGraphViewModel measurementsGraphViewModel = new MeasurementGraphViewModel();
        private BindableBase currentViewModel;
        private Stack<object> history;

        public MyICommand<string> NavCommand { get; private set; }
        public MyICommand BackCommand { get; set; }

        public BindableBase CurrentViewModel
        {
            get
            {
                return currentViewModel;
            }

            set
            {
                SetProperty(ref currentViewModel, value);
            }
        }
        private void OnNav(string destination)
        {
            switch (destination)
            {
                //Uraditi i za back funkciju
                case "Network Entities":
                    CurrentViewModel = networkEntitiesViewModel;
                    break;
                case "Network Display":
                    CurrentViewModel = networkDisplayViewModel;
                    break;
                case "Measurment Graph":
                    CurrentViewModel = measurementsGraphViewModel;
                    break;
            }
        }
    }
}
