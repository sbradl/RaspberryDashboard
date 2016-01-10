using SimRacingDashboard.Entities;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace SimRacingDashboard.Profiler.Interactors.IO
{
    public class OpenInteractor
    {
        public IList<TelemetryDataSet> Execute(string filename)
        {
            //var xmlserializer = new XmlSerializer(typeof(List<TelemetryDataSet>));
            //using (var reader = File.OpenText(filename))
            //{
            //    return (IList<TelemetryDataSet>) xmlserializer.Deserialize(reader);
            //}

            var formatter = new BinaryFormatter();
            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return (IList<TelemetryDataSet>)formatter.Deserialize(stream);
            }
        }
    }
}
