using System;

namespace SimRacingDashboard.Entities
{
    [Serializable]
    public struct Pressure
    {
        public float Value { get;set;}
        public PressureUnit Unit { get; set; }
    }
}

