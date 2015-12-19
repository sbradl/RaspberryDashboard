using System;
using SimRacingDashboard.Entities;

namespace SimRacingDashboard.Output
{
    public interface ICarStateVisualizer
    {
        void Visualize(CarState carState);
    }
}

