using SimRacingDashboard.DataAccess.Udp;
using SimRacingDashboard.Entities;

namespace SimRacingDashboard.DataAccess.PCars
{
    public class TelemetryGateway : AbstractTelemetryGateway
    {
        private PacketParser parser = new PacketParser();

        public TelemetryGateway()
            : base(5606)
        {
        }

        protected override TelemetryDataSet? Parse(byte[] data)
        {
            return this.parser.Parse(data);
        }
    }
}

