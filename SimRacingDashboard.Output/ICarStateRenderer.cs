using SimRacingDashboard.Entities;

namespace SimRacingDashboard.Output
{
    public interface ICarStateRenderer
    {
        void Render(CarState carState);
    }
}

