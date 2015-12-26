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

            try
            {
                foreach (var renderer in renderers)
                {
                    gateway.TelemetryChanged += (sender, carState) => renderer.Render(carState);
                }

                gateway.StartReading();

                Console.WriteLine("Press any key to stop...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            gateway.Shutdown();

            foreach (var renderer in renderers)
            {
                renderer.Dispose();
            }
        }

        private static ITelemetryGateway CreateGateway(string backend)
        {
           return GetServiceInstanceFrom<ITelemetryGateway>(backend);
        }

        private static IEnumerable<ITelemetryRenderer> CreateRenderers(string frontends)
        {
            string[] frontendNames = frontends.Split(',');

            return frontendNames
                .Select(name => GetServiceInstanceFrom<ITelemetryRenderer>(name))
                .ToList();
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
