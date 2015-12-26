using SimRacingDashboard.Entities;
using SimRacingDashboard.Output.Dashboard.Gpio;

namespace SimRacingDashboard.Output.Dashboard
{
    public class Dashboard : ITelemetryRenderer
    {
        private OutputPin shiftLight;

        public Dashboard()
        {
            this.shiftLight = new OutputPin(25);    
        }

        public void Dispose()
        {
            this.shiftLight.Dispose();
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
        }
    }
}
