using System;
using System.Reflection;

namespace Sumo.Nop.MediaTools.Common {
    public static class MethodTimeLogger {
        public static void Log(MethodBase methodBase, long milliseconds) {
            var t = TimeSpan.FromMilliseconds(milliseconds);
            var timeElapsed = $"{t.Hours:D2}h:{t.Minutes:D2}m:{t.Seconds:D2}s:{t.Milliseconds:D3}ms";
            Console.WriteLine();
            Console.WriteLine($"{methodBase} execution time: {timeElapsed}");
        }
    }
}