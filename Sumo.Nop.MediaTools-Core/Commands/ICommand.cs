using Microsoft.Extensions.CommandLineUtils;

namespace Sumo.Nop.MediaToolsCore.Commands {
    public interface ICommand {
        void Run(CommandLineApplication command);
    }
}