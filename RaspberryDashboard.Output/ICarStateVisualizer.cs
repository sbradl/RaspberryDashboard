using System;
using RaspberryDashboard.Entities;

namespace RaspberryDashboard.Output
{
    public interface ICarStateVisualizer
    {
        void Visualize(CarState carState);
    }
}

