using Newtonsoft.Json;

namespace SimRacingDashboard.Output.Console
{
    public class ConsoleRenderer : ICarStateRenderer
    {
        public void Render(Entities.CarState carState)
        {
            var json = JsonConvert.SerializeObject(carState, Formatting.Indented);

            System.Console.WriteLine(json);
        }
    }
}

