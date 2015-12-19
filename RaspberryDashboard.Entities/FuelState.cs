namespace RaspberryDashboard.Entities
{
    public struct FuelState
    {
        public Pressure FuelPressure { get; set; }

        public uint CapacityInLiters { get; set; }

        public float Level { get; set; }

        public float RemainingLiters
        {
            get
            {
                return this.CapacityInLiters * this.Level;
            }
        }
    }
}

