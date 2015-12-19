using System;

namespace RaspberryDashboard.Entities
{
    public struct WaterState
    {
        public Temperature Temperature { get; set; }
        public Pressure Pressure {get;set;}
    }
}

