using SimRacingDashboard.Entities;
using System;

namespace SimRacingDashboard.Output.Beep
{
    public class Beeper : ICarStateRenderer
    {
        private CarState? oldState = null;
        private CarState newState;

        public void Render(CarState carState)
        {
            if(this.oldState == null)
            {
                this.oldState = carState;
            }

            this.newState = carState;

            Process();

            this.oldState = this.newState;
        }

        private void Process()
        {
            Console.WriteLine("RPM: {0} / {1} - {2}", this.newState.Engine.Rpm, this.newState.Engine.MaxRpm, this.newState.Engine.RpmLevel);
            // rpm shiftindicator - short beep
            if(this.oldState.Value.Engine.Shift != this.newState.Engine.Shift && this.newState.Engine.Shift)
            {
                Console.Beep(4000, 500);
            }

            // shift - one beep for every gear
            if(this.oldState.Value.GearBox.CurrentGear != this.newState.GearBox.CurrentGear)
            {
                if(this.newState.GearBox.CurrentGear == 15)
                {
                    Console.Beep(500, 1000);
                }
                else
                {
                    for (int i = 0; i < this.newState.GearBox.CurrentGear; ++i)
                    {
                        Console.Beep(1000, 250);
                    }
                }
            }

            var tires = this.newState.Tires;
            Console.WriteLine("Ride Height: {0}, {1}, {2}, {3}", tires.FrontLeft.RideHeight, tires.FrontRight.RideHeight, tires.RearLeft.RideHeight, tires.RearRight.RideHeight);
            
            if(this.oldState.Value.Tires.CarTouchesGround != this.newState.Tires.CarTouchesGround && this.newState.Tires.CarTouchesGround)
            {
                Console.Beep(4000, 500);
            }
        }
    }
}
