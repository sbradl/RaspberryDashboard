using SimRacingDashboard.Output.Dashboard.Gpio;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Threading;

namespace SimRacingDashboard.Output.Dashboard.SPI
{
    public class Connection : IDisposable
    {
        private OutputPin dataPin;
        private OutputPin clock;
        private OutputPin load;
        
        public Connection(int dataPin, int clockPin, int loadPin)
        {
            this.dataPin = new OutputPin(dataPin);
            this.clock = new OutputPin(clockPin);
            this.load = new OutputPin(loadPin);
        }

        public void Dispose()
        {
            this.dataPin.Dispose();
            this.clock.Dispose();
            this.load.Dispose();
        }

        public void StartTransfer()
        {
            this.load.Disable();
        }

        public void FinishTransfer()
        {
            this.load.Enable();
        }

        public void WriteData(byte data)
        {
            string s = Convert.ToString(data, 2);
            char[] x = s.PadLeft(8, '0').ToCharArray();

            bool[] bits = x
                .Select(b => b == '1' ? true : false)
                .ToArray();

            foreach (bool bit in bits)
            {
                if (bit)
                {
                    this.dataPin.Enable();
                }
                else
                {
                    this.dataPin.Disable();
                }

                this.clock.Enable();
                this.clock.Disable();
            }
        }
    }
}
