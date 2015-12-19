using System;

namespace RaspberryDashboard.Entities
{
    public struct EngineState
    {
        public bool IsRunning { get; set; }
        
        public ushort Rpm { get; set; }
        public ushort MaxRpm { get; set; }

        public float Level
        {
            get
            {
                return this.Rpm / this.MaxRpm;
            }
        }

        public float BoostAmount { get; set; }

        public float Speed { get; set; }
        public float Torque { get; set; }
    }
}

