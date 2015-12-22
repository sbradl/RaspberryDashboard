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
            // rpm shiftindicator
            if (this.oldState.Value.Engine.Shift != this.newState.Engine.Shift && this.newState.Engine.Shift)
            {
                Console.Beep(1000, 250);
            }

            var tires = this.newState.Tires;

            if (this.oldState.Value.Tires.CarTouchesGround != this.newState.Tires.CarTouchesGround && this.newState.Tires.CarTouchesGround)
            {
                Console.Beep(4000, 500);
            }
        }
    }
}
