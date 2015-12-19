using System;

namespace SimRacingDashboard.Entities
{
    [Serializable]
    public struct OilState
    {
        public Temperature Temperature {get;set;}
        public Pressure Pressure {get;set;}
    }
}

