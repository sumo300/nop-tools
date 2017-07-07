using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Sumo.Nop.MediaToolsCore.Commands;

namespace Sumo.Nop.MediaToolsCore {
    internal class Program {
        private static IConfigurationRoot Configuration { get; set; }

        private static void Main(string[] args) {
            SetUpConfig();

            var app = new CommandLineApplication {Name = "nop-tools"};

            app.HelpOption("-?|-h|--help");

            app.OnExecute(() => {
                Console.WriteLine("Hello World!");
                return 0;
            });

            AddCommands(app);

            app.Execute(args);
        }

        private static void AddCommands(CommandLineApplication app) {
            foreach (var com in GetCommands()) {
                var className = com.GetType().Name;
                app.Command(className, com.Run);
            }
        }

        private static List<ICommand> GetCommands() {
            var asy = Assembly.GetEntryAssembly();
            return (from ti in asy.DefinedTypes
                where ti.ImplementedInterfaces.Contains(typeof(ICommand))
                select asy.CreateInstance(ti.FullName) as ICommand).ToList();
        }

        private static void SetUpConfig() {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }
    }
}