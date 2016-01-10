using SimRacingDashboard.Output.Dashboard.Gpio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimRacingDashboard.Output.Dashboard
{
    public class SevenSegmentDisplay : IDisposable
    {
        private OutputPin D0;
        private OutputPin D1;
        private OutputPin D2;
        private OutputPin D3;
        private OutputPin Latch;

        public SevenSegmentDisplay()
        {
            this.D0 = new OutputPin(5);
            this.D1 = new OutputPin(6);
            this.D2 = new OutputPin(13);
            this.D3 = new OutputPin(19);
            this.Latch = new OutputPin(26);
            this.Latch.Enable();
        }

        public void Dispose()
        {
            this.D0.Dispose();
            this.D1.Dispose();
            this.D2.Dispose();
            this.D3.Dispose();
            this.Latch.Disable();
        }

        public void Write(byte number)
        {
            this.D0.Enable();
            this.D1.Disable();
            this.D2.Enable();
            this.D3.Disable();
        }
    }
}
