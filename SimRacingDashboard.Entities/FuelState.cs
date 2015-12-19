namespace SimRacingDashboard.Entities
{
    public struct FuelState
    {
        public Pressure Pressure { get; set; }

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

