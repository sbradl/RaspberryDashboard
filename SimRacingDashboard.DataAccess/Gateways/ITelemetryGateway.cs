using System;
using SimRacingDashboard.Entities;

namespace SimRacingDashboard.DataAccess
{
    public interface ITelemetryGateway
    {
        event EventHandler<TelemetryDataSet> TelemetryChanged;

        void StartReading();

        void Shutdown();
    }
}

