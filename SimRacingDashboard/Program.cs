using System;
using SimRacingDashboard.DataAccess;
using SimRacingDashboard.Output;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Configuration;

namespace SimRacingDashboard
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                args = new [] {
                    ConfigurationManager.AppSettings["DataAccessPlugin"],
                    ConfigurationManager.AppSettings["OutputPlugins"]
                };
            }

            Console.WriteLine("Starting {0}", AppDomain.CurrentDomain.FriendlyName);

            var backend = args[0];
            var frontends = args[1];

            var gateway = CreateGateway(backend);
            var renderers = CreateRenderers(frontends);

            foreach (var renderer in renderers)
            {
                gateway.CarStateChanged += (sender, carState) => renderer.Render(carState);
            }

            gateway.StartReading();

            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();

            gateway.Shutdown();
        }

        private static ICarStateGateway CreateGateway(string backend)
        {
           return GetServiceInstanceFrom<ICarStateGateway>(backend);
        }

        private static IEnumerable<ICarStateRenderer> CreateRenderers(string frontends)
        {
            string[] frontendNames = frontends.Split(',');

            return frontendNames.Select(name => GetServiceInstanceFrom<ICarStateRenderer>(name));
        }

        private static T GetServiceInstanceFrom<T>(string pluginName)
        {
            var plugin = LoadPlugin(pluginName);
            var type = plugin.ExportedTypes.First(t => t.GetInterfaces().Contains(typeof(T)));

            return (T)Activator.CreateInstance(type);
        }

        private static Assembly LoadPlugin(string name)
        {
            var dll = Path.GetFullPath(name + ".dll");
            var assembly = Assembly.LoadFile(dll);

            return assembly;
        }
    }
}
