using System;
using RaspberryDashboard.DataAccess;
using RaspberryDashboard.Output;
using RaspberryDashboard.Output.Console;

namespace RaspberryDashboard
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                args = new [] { "PCars", "Console" };
            }

            Console.WriteLine("Starting RaspberryDashboard");

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
                return new RaspberryDashboard.DataAccess.PCars.CarStateGateway();
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
