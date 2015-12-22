using System;

namespace SimRacingDashboard.Entities
{
    [Serializable]
    public struct Tire
    {
        public float RideHeightInMeter { get; set; }

        public float HeightAboveGround { get; set; }

        public float SlipSpeed { get; set; }

        public float GripLevel { get; set; }

        public float WearLevel { get; set; }

        public Pressure Pressure { get; set; }
    }
}

