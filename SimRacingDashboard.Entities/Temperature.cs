using System;

namespace SimRacingDashboard.Entities
{
    [Serializable]
    public struct Temperature
    {
        public float Value { get; set;}
        public TemperatureUnit Unit { get; set; }
    }
}

