using System;

namespace SimRacingDashboard.Entities
{
    [Serializable]
    public struct TelemetryDataSet
    {
        public ushort Version { get; set; }

        public DateTime DateTime { get; set; }

        public SessionInfo Session { get; set; }

        public EventState Event { get; set; }

        public TimingInfo Timings { get; set; }

        public OilState Oil { get; set;}

        public WaterState Water { get; set; }

        public FuelState Fuel { get; set; }

        public float Speed { get; set; }

        public EngineState Engine { get; set; }

        public GearBoxState GearBox { get; set; }

        public Tires Tires { get; set; }

        public ControlLightsState ControlLights { get; set; }

        public short[] Position { get; set; }

        public float CurrentTrackDistance { get; set; }

        public byte LapsCompleted { get; set; }
        public byte CurrentLap { get; set; }
    }
}

