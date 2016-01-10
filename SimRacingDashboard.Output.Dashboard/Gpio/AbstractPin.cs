using System;
using System.IO;

namespace SimRacingDashboard.Output.Dashboard.Gpio
{
    public abstract class AbstractPin : IDisposable
    {
        private const string BasePath = "/sys/class/gpio";
        private const string ExportPath = BasePath + "/export";
        private const string UnExportPath = BasePath + "/unexport";

        private int pinNumber;
        private string pinTarget;

        protected AbstractPin(int pinNumber)
        {
            this.pinNumber = pinNumber;
            this.pinTarget = Path.Combine(BasePath, "gpio" + pinNumber);
            this.IsInput = true;

            ExportPin();
        }

        protected bool IsInput
        {
            get;private set;
        }

        protected bool IsOutput
        {
            get
            {
                return !IsInput;
            }
        }

        protected bool IsActive
        {
            get; private set;
        }

        public void Dispose()
        {
            SetPinAsInput();
            UnExportPin();
        }

        protected void SetPinAsOutput()
        {
            Console.WriteLine("Set pin {0} as output", this.pinNumber);
            Write("out", Path.Combine(this.pinTarget, "direction"));
            this.IsInput = false;
        }

        protected void SetPinAsInput()
        {
            Console.WriteLine("Set pin {0} as input", this.pinNumber);
            Write("in", Path.Combine(this.pinTarget, "direction"));
            this.IsInput = true;
        }

        protected void WriteValue(bool value)
        {
            if(this.IsInput)
            {
                throw new InvalidOperationException(string.Format("Pin {0} is in input mode", this.pinNumber));
            }

            Write(value ? "1" : "0", Path.Combine(this.pinTarget, "value"));

            this.IsActive = value;
        }

        private void ExportPin()
        {
            Console.WriteLine("Exporting pin {0}", this.pinNumber);
            Write(this.pinNumber.ToString(), ExportPath);
        }

        private void UnExportPin()
        {
            Console.WriteLine("Unexporting pin {0}", this.pinNumber);
            Write(this.pinNumber.ToString(), UnExportPath);
        }

        private void Write(string data, string fileName)
        {
            using (var file = new FileInfo(fileName).OpenWrite())
            using (var writer = new StreamWriter(file))
            {
                writer.Write(data);
                writer.Flush();
            }
        }
    }
}
