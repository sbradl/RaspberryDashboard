using SimRacingDashboard.DataAccess;
using System;
using System.Configuration;

namespace SimRacingDashboard.Profiler
{
    static class GatewayFactory
    {
        public static ITelemetryGateway CreateGateway()
        {
            switch(ConfigurationManager.AppSettings["Game"])
            {
                case "PCars":
                    return new DataAccess.PCars.Gateways.TelemetryGateway();

                case "DirtRally":
                    return new DataAccess.DirtRally.Gateways.TelemetryGateway();

                default:
                    throw new ArgumentException("Unsupported game specified");
            }
        }
    }
}
