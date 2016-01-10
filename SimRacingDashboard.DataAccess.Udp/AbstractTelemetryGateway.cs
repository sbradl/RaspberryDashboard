using System;
using SimRacingDashboard.Entities;
using System.Threading;
using System.Net;

namespace SimRacingDashboard.DataAccess.Udp
{
    public abstract class AbstractTelemetryGateway : ITelemetryGateway
    {
        public event EventHandler<TelemetryDataSet> TelemetryChanged;

        private volatile bool isRunning;

        private Thread worker;

        private UdpReader udpClient;
       // private UdpReplayer udpClient;

        private int port;
        private IPAddress ip;

        protected AbstractTelemetryGateway(int udpPort)
            : this(udpPort, IPAddress.Any)
        {
        }

        protected AbstractTelemetryGateway(int udpPort, IPAddress ip)
        {
            this.port = udpPort;
            this.ip = ip;
        }

        public void StartReading()
        {
            this.udpClient = new UdpReader(this.port, this.ip);
            // this.udpClient = new UdpReplayer();
            this.worker = new Thread(ReadData);
            this.worker.Start();
            this.isRunning = true;
        }

        public void Shutdown()
        {
            if(!this.isRunning)
            {
                return;
            }

            this.isRunning = false;

            try
            {
                this.udpClient.Dispose();
            }
            catch (ObjectDisposedException)
            {

            }

            if (!this.worker.Join(TimeSpan.FromSeconds(1)))
            {
                this.worker.Abort();
            }
        }

        protected abstract TelemetryDataSet? Parse(byte[] data);

        private void ReadData()
        {
            //using (var stream = new FileStream("telemetry.udp", FileMode.Create, FileAccess.Write, FileShare.None))
            //using(var writer = new BinaryWriter(stream))
            {
                while (this.isRunning)
                {
                    var data = udpClient.ReadData();

                    if (!this.isRunning)
                    {
                        return;
                    }

                    var telemetryDataSet = Parse(data);

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
