using SimRacingDashboard.DataAccess;
using System;
using SimRacingDashboard.Entities;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace SimRacingDashboard.Profiler.Gateways
{
    class FileGateway : ICarStateGateway
    {
        public event EventHandler<CarState> CarStateChanged;
        
        public void StartReading()
        {
            var formatter = new BinaryFormatter();

            using (var stream = new FileStream("telemetry.bin", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                while (stream.Position < stream.Length)
                {
                    Console.WriteLine("Reading");
                    var carState = (CarState)formatter.Deserialize(stream);

                    if (this.CarStateChanged != null)
                    {
                        this.CarStateChanged(this, carState);
                    }
                }
            }
        }

        public void Shutdown()
        {
            
        }
    }
}
