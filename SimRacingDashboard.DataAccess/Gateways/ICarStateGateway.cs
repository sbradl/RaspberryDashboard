using System;
using SimRacingDashboard.Entities;

namespace SimRacingDashboard.DataAccess
{
    public interface ICarStateGateway
    {
        event EventHandler<CarState> CarStateChanged;

        void StartReading();

        void Shutdown();
    }
}

