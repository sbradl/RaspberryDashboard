using System;
using RaspberryDashboard.Entities;
using System.IO;

namespace RaspberryDashboard.DataAccess.PCars
{
    public class PacketParser
    {
        public CarState Parse(byte[] data)
        {
            using (var memoryStream = new MemoryStream(data))
            using (var binaryReader = new BinaryReader(memoryStream))
            {
                return new Helper(binaryReader).ReadCarState();
            }
        }

        private class Helper
        {
            private BinaryReader reader;
            private CarState carState = new CarState();

            public Helper(BinaryReader reader)
            {
                this.reader = reader;  
            }

            public CarState ReadCarState()
            {
                ReadOilState();
                ReadEngineState();

                return this.carState;
            }

            private void ReadOilState()
            {
                Goto(100);
                this.carState.Oil = new OilState
                {
                    Temperature = ReadTemperature(),
                    Pressure = ReadPressure()
                };
            }

            private void ReadEngineState()
            {
                Goto(124);
                this.carState.Engine = new EngineState
                {
                        Rpm = this.reader.ReadUInt16(),
                        MaxRpm = this.reader.ReadUInt16()
                };
            }

            private void Goto(long offset)
            {
                this.reader.BaseStream.Seek(offset, SeekOrigin.Begin);
            }

            private Temperature ReadTemperature()            
            {
                return new Temperature
                {
                    Value = (float) this.reader.ReadInt16(),
                    Unit = TemperatureUnit.Celsius
                };
            }

            private Pressure ReadPressure()
            {
                return new Pressure
                {
                    Value = this.reader.ReadUInt16(),
                    Unit = PressureUnit.KPa
                };
            }
        }
    }
}

