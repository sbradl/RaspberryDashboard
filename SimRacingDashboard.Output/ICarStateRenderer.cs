using SimRacingDashboard.Entities;

namespace SimRacingDashboard.Output
{
    public interface ICarStateRenderer
    {
        void Render(TelemetryDataSet carState);
    }
}

