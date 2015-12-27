using SimRacingDashboard.Entities;
using SimRacingDashboard.DataAccess.Udp;
using System.Net;

namespace SimRacingDashboard.DataAccess.DirtRally.Gateways
{
    // assumes udp to be enabled for port 20777 on ip 127.0.0.1 with extradata=3
    public class TelemetryGateway : AbstractTelemetryGateway
    {
        private PacketParser parser = new PacketParser();

        public TelemetryGateway()
            : base(20777, IPAddress.Any)
        {
        }

        protected override TelemetryDataSet? Parse(byte[] data)
        {
            if(data.Length != 264)
            {
                return null;
            }
            
            return this.parser.Parse(data);
        }
    }
}
