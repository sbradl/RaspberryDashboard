using SimRacingDashboard.Entities;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace SimRacingDashboard.Output.File
{
    public class FileRenderer : ICarStateRenderer
    {
        private BinaryFormatter formatter = new BinaryFormatter();

        public void Render(TelemetryDataSet carState)
        {
            using (var stream = new FileStream("telemetry.bin", FileMode.Append, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, carState);
            }
        }
    }
}
