using System;
using System.Threading;

namespace SimRacingDashboard.DataAccess.PCars
{
    public class CarStateGateway : ICarStateGateway
    {
        public event EventHandler<Entities.CarState> CarStateChanged;

        private volatile bool isRunning = true;

        private Thread worker;

        public CarStateGateway()
        {
            this.worker = new Thread(ReadData);
        }

        public void StartReading()
        {
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

            using (var udpClient = new UdpReader(5606))
            {
                while (this.isRunning)
                {
                    var data = udpClient.ReadData();

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

