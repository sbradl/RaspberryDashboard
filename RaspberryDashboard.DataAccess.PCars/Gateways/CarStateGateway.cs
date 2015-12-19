using System;
using System.Threading;

namespace SimRacingDashboard.DataAccess.PCars
{
    public class CarStateGateway : ICarStateGateway
    {
        public event EventHandler<SimRacingDashboard.Entities.CarState> CarStateChanged;

        private volatile bool isRunning = true;

        private Thread worker;

        public CarStateGateway()
        {
            this.worker = new Thread(ReadData);
            this.worker.Start();
        }

        public void Shutdown()
        {
            this.isRunning = false;

            if (!this.worker.Join(TimeSpan.FromSeconds(3)))
            {
                this.worker.Abort();
            }
        }

        private void ReadData()
        {
            var parser = new PacketParser();

            bool recorded = false;

            using (var udpClient = new UdpReader(5606))
            {
                while (this.isRunning)
                {
                    Console.WriteLine("Waiting for data...");
                    var data = udpClient.ReadData();

                    if (!recorded)
                    {
                        System.IO.File.WriteAllBytes("udp.bin", data);
                        recorded = true;
                    }

                    Console.WriteLine("Data: {0} bytes", data.Length);

                    var carState = parser.Parse(data);

                    if (this.CarStateChanged != null)
                    {
                        this.CarStateChanged(this, carState);
                    }
                }
            }
        }
    }
}

