using System;
using Newtonsoft.Json;

namespace SimRacingDashboard.Output.Console
{
    public class ConsoleVisualizer : ICarStateVisualizer
    {
        public void Visualize(SimRacingDashboard.Entities.CarState carState)
        {
            var json = JsonConvert.SerializeObject(carState, Formatting.Indented);

            System.Console.WriteLine(json);
        }
    }
}

