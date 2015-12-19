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
                return (new float[] { FrontLeft.RideHeight, FrontRight.RideHeight, RearLeft.RideHeight, RearRight.RideHeight }).Min();
            }
        }

        public bool CarTouchesGround
        {
            get
            {
                return FrontLeft.RideHeight <= 0 || FrontRight.RideHeight <= 0 ||
                    RearLeft.RideHeight <= 0 || RearRight.RideHeight <= 0;
            }
        }
    }
}

