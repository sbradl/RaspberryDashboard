namespace RaspberryDashboard.Entities
{
    public struct CarState
    {
        public OilState Oil { get; set;}

        public WaterState Water { get; set; }

        public FuelState Fuel { get; set; }

        public float Speed { get; set; }

        public EngineState Engine { get; set; }

        public GearBoxState GearBox { get; set; }

        public ControlLightsState ControlLights { get; set; }
    }
}

