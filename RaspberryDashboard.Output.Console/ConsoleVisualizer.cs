using System;
using Newtonsoft.Json;

namespace RaspberryDashboard.Output.Console
{
    public class ConsoleVisualizer : ICarStateVisualizer
    {
        public void Visualize(RaspberryDashboard.Entities.CarState carState)
        {
            var json = JsonConvert.SerializeObject(carState, Formatting.Indented);

            System.Console.WriteLine(json);
        }
    }
}

