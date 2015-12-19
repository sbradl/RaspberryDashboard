using System;
using RaspberryDashboard.Entities;

namespace RaspberryDashboard.DataAccess
{
    public interface ICarStateGateway
    {
        event EventHandler<CarState> CarStateChanged;

        void Shutdown();
    }
}

