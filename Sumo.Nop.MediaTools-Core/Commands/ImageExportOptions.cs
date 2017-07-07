using Microsoft.Extensions.CommandLineUtils;

namespace Sumo.Nop.MediaToolsCore.Commands
{
    public class ImageExportOptions
    {
        public CommandOption OutputDirectory { get; set; }

        public CommandOption IsUpdateEnabled { get; set; }

        public CommandOption StoreId { get; set; }
    }
}