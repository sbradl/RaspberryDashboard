using SimRacingDashboard.Entities;
using System;

namespace SimRacingDashboard.Output
{
    public interface ITelemetryRenderer : IDisposable
    {
        void Render(TelemetryDataSet telemetryDataset);
    }
}

