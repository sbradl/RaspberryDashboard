using System;

namespace SimRacingDashboard.Entities
{
    [Serializable]
    public struct CarState
    {
        public DateTime DateTime { get; set; }

        public OilState Oil { get; set;}

        public WaterState Water { get; set; }

        public FuelState Fuel { get; set; }

        public float Speed { get; set; }

        public EngineState Engine { get; set; }

        public GearBoxState GearBox { get; set; }

        public Tires Tires { get; set; }

        public ControlLightsState ControlLights { get; set; }
    }
}

