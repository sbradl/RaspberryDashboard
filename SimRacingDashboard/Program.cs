using System;
using SimRacingDashboard.DataAccess;
using SimRacingDashboard.Output;
using SimRacingDashboard.Output.Console;

namespace SimRacingDashboard
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                args = new [] { "PCars", "Console" };
            }

            Console.WriteLine("Starting {0}", System.AppDomain.CurrentDomain.FriendlyName);

            var backend = args[0];
            var frontend = args[1];

            var gateway = CreateGateway(backend);
            var visualizer = CreateVisualizer(frontend);
            gateway.CarStateChanged += (sender, carState) => visualizer.Visualize(carState);

            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine("Stopping...");

            gateway.Shutdown();
        }

        private static ICarStateGateway CreateGateway(string backend)
        {
            if (backend == "PCars")
            {
                return new DataAccess.PCars.CarStateGateway();
            } 

            throw new ArgumentException("Invalid backend: " + backend);
        }

        private static ICarStateVisualizer CreateVisualizer(string frontend)
        {
            if (frontend == "Console")
            {
                return new ConsoleVisualizer();
            }

            throw new ArgumentException("Invalid frontend: " + frontend);
        }
    }
}
