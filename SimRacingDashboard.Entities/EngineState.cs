using System;

namespace SimRacingDashboard.Entities
{
    [Serializable]
    public struct EngineState
    {
        public bool IsRunning { get; set; }
        
        public ushort Rpm { get; set; }
        public ushort MaxRpm { get; set; }

        public float RpmLevel
        {
            get
            {
                if(this.MaxRpm == 0)
                {
                    return 0;
                }

                return this.Rpm / (float)this.MaxRpm;
            }
        }

        public bool Shift
        {
            get
            {
                return this.RpmLevel > 0.95;
            }
        }

        public float BoostAmount { get; set; }

        public float Speed { get; set; }
        public float Torque { get; set; }
    }
}

