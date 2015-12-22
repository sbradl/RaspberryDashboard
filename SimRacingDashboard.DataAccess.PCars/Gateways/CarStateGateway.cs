using System;
using System.IO;
using System.Threading;

namespace SimRacingDashboard.DataAccess.PCars
{
    public class CarStateGateway : ICarStateGateway
    {
        public event EventHandler<Entities.CarState> CarStateChanged;

        private volatile bool isRunning = true;

        private Thread worker;

        //private UdpReader udpClient = new UdpReader(5606);
        private UdpReplayer udpClient = new UdpReplayer();

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

            this.udpClient.Dispose();

            if (!this.worker.Join(TimeSpan.FromSeconds(1)))
            {
                this.worker.Abort();
            }
        }

        private void ReadData()
        {
            var parser = new PacketParser();
            
            //using (var stream = new FileStream("telemetry.udp", FileMode.Create, FileAccess.Write, FileShare.None))
            //using(var writer = new BinaryWriter(stream))
            {
                while (this.isRunning)
                {
                    var data = udpClient.ReadData();

                    if(!this.isRunning)
                    {
                        return;
                    }

                    var carState = parser.Parse(data);

                    if (carState.HasValue && this.CarStateChanged != null)
                    {
                        //writer.Write(data);
                        this.CarStateChanged(this, carState.Value);
                    }
                }
            }
        }
    }
}

