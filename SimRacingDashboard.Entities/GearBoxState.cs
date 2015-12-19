using System;

namespace SimRacingDashboard.Entities
{
    [Serializable]
    public struct GearBoxState
    {
        public byte CurrentGear { get; set; }
        public byte NumGears { get; set; }
    }
}

