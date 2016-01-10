using SimRacingDashboard.Entities;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SimRacingDashboard.Profiler.Interactors.IO
{
    public class SaveInteractor
    {
        public void Execute(string filename, IList<TelemetryDataSet> datasets)
        {
            var dataToWrite = new List<TelemetryDataSet>(datasets);

            //var xmlserializer = new XmlSerializer(typeof(List<TelemetryDataSet>), new[] { typeof(TimingInfo), typeof(TimeSpan) });
            //using (var stringWriter = new StringWriter())
            //using (var writer = new XmlTextWriter(stringWriter) { Formatting = Formatting.Indented })
            //{
            //    xmlserializer.Serialize(writer, dataToWrite);
            //    var xml = stringWriter.ToString();

            //    File.WriteAllText(filename, xml);
            //}

            var formatter = new BinaryFormatter();
            using (var stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, dataToWrite);
            }
        }
    }
}
