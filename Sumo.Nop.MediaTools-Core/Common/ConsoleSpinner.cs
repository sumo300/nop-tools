using System;

namespace Sumo.Nop.MediaToolsCore.Common {
    public class ConsoleSpinner {
        private int _counter;

        public ConsoleSpinner() {
            _counter = 0;
        }

        public void Turn() {
            _counter++;
            switch (_counter % 4) {
                case 0:
                    Console.Write("/");
                    break;
                case 1:
                    Console.Write("-");
                    break;
                case 2:
                    Console.Write("\\");
                    break;
                case 3:
                    Console.Write("|");
                    break;
            }
            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
        }
    }
}