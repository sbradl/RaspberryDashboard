using System;

namespace SimRacingDashboard.Entities
{
    [Serializable]
    public struct WaterState
    {
        public Temperature Temperature { get; set; }
        public Pressure Pressure {get;set;}
    }
}

