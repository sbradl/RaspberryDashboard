using System;

namespace SimRacingDashboard.Entities
{
    public struct EngineState
    {
        public bool IsRunning { get; set; }
        
        public ushort Rpm { get; set; }
        public ushort MaxRpm { get; set; }

        public float RpmLevel
        {
            get
            {
                return this.Rpm / this.MaxRpm;
            }
        }

        public bool Shift
        {
            get
            {
                return this.RpmLevel >= 0.9;
            }
        }

        public float BoostAmount { get; set; }

        public float Speed { get; set; }
        public float Torque { get; set; }
    }
}

