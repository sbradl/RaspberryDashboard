using System;
using System.IO;
using System.Threading;

namespace SimRacingDashboard.DataAccess.PCars
{
    public class TelemetryGateway : ITelemetryGateway
    {
        public event EventHandler<Entities.TelemetryDataSet> TelemetryChanged;

        private volatile bool isRunning = true;

        private Thread worker;

        private UdpReader udpClient = new UdpReader(5606);
        //private UdpReplayer udpClient = new UdpReplayer();

        public TelemetryGateway()
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

                    var telemetryDataSet = parser.Parse(data);

                    if (telemetryDataSet.HasValue && this.TelemetryChanged != null)
                    {
                        //writer.Write(data);
                        this.TelemetryChanged(this, telemetryDataSet.Value);
                    }
                }
            }
        }
    }
}

