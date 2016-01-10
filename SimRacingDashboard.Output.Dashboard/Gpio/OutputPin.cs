namespace SimRacingDashboard.Output.Dashboard.Gpio
{
    public class OutputPin : AbstractPin
    {
        public OutputPin(int pinNumber)
            : base(pinNumber)
        {
            SetPinAsOutput();
        }

        public void Enable()
        {
            if(this.IsActive)
            {
                return;
            }

            WriteValue(true);
        }

        public void Disable()
        {
            if (!this.IsActive)
            {
                return;
            }

            WriteValue(false);
        }
    }
}
