using SimRacingDashboard.Entities;
using SimRacingDashboard.Output.Dashboard.Gpio;

namespace SimRacingDashboard.Output.Dashboard
{
    public class Dashboard : ITelemetryRenderer
    {
        private OutputPin shiftLight;
        private Max7219 gear;

        public Dashboard()
        {
            this.shiftLight = new OutputPin(26);
            this.gear = new Max7219(17, 22, 25);
        }

        public void Dispose()
        {
            this.shiftLight.Dispose();
            this.gear.Dispose();
        }

        public void Render(TelemetryDataSet dataset)
        {
            if(dataset.Engine.Shift)
            {
                this.shiftLight.Enable();
            }
            else
            {
                this.shiftLight.Disable();
            }

            this.gear.Display(1, dataset.GearBox.CurrentGear);
        }
    }
}
