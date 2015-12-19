using System;

namespace SimRacingDashboard.Entities
{
    [Serializable]
    public struct DrivingAssistsState
    {
        public bool ABS { get; set; }
        public bool StabilityControl { get; set; }
        public bool TractionControl { get; set; }
    }
}

