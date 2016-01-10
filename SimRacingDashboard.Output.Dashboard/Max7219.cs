using SimRacingDashboard.Output.Dashboard.SPI;
using System;

namespace SimRacingDashboard.Output.Dashboard
{
    public class Max7219 : IDisposable
    {
        private const byte DECODE_MODE = 0x09;
        private const byte INTENSITY = 0x0a;
        private const byte SCAN_LIMIT = 0x0b;
        private const byte SHUTDOWN = 0x0c;
        private const byte DISPLAY_TEST = 0x0f;

        private Connection spi;

        public Max7219(int dataPin, int clockPin, int loadPin)
        {
            this.spi = new Connection(dataPin, clockPin, loadPin);

            Write(SHUTDOWN, 0x00);
            Write(SCAN_LIMIT, 0x07);
            Write(INTENSITY, 0x04);
            Write(DECODE_MODE, 0xff);
            Write(SHUTDOWN, 0x01);
        }

        public void Dispose()
        {
            Write(SHUTDOWN, 0x00);
            this.spi.Dispose();
        }

        public void Display(byte digit, byte number)
        {
            Write(digit, number);
        }

        private void Write(byte register, byte data)
        {
            this.spi.StartTransfer();
            this.spi.WriteData(register);
            this.spi.WriteData(data);
            this.spi.FinishTransfer();
        }
    }
}
