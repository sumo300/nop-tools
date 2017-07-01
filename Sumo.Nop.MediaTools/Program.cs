using System;
using ManyConsole;

namespace Sumo.Nop.MediaTools
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var commands = ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof(Program));
            ConsoleCommandDispatcher.DispatchCommand(commands, args, Console.Out, true);
        }
    }
}