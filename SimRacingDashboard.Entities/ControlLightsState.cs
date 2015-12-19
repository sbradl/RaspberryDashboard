using System;

namespace SimRacingDashboard.Entities
{
    [Serializable]
    public struct ControlLightsState
    {
        public DrivingAssistsState DrivingAssists { get; set; }

        public bool LightsAreOn { get; set; }

        public bool EngineWarning { get; set; }

        public bool SpeedLimiter { get; set; }

        public bool Handbrake { get; set; }
    }
}

