using System;
using System.Linq;

namespace SimRacingDashboard.Entities
{
    [Serializable]
    public struct Tires
    {
        public Tire FrontLeft {get;set;}
        public Tire FrontRight {get;set;}
        public Tire RearLeft {get;set;}
        public Tire RearRight {get;set;}

        public float MinRideHeight
        {
            get
            {
                return (new float[] { FrontLeft.RideHeightInMeter, FrontRight.RideHeightInMeter, RearLeft.RideHeightInMeter, RearRight.RideHeightInMeter }).Min();
            }
        }

        public bool CarTouchesGround
        {
            get
            {
                return this.MinRideHeight <= 0;
            }
        }
    }
}

