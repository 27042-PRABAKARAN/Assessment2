using Boiler.Models.Enums;

namespace Boiler.Models
{
    internal class BoilerSystem
    {
        public BoilerSystem(SystemStatus systemStatus, SwitchStatus switchStatus)
        {
            this.SwitchStatus = switchStatus;
            this.SystemStatus = systemStatus;
        }
        public SystemStatus SystemStatus { get; set; }
        public SwitchStatus SwitchStatus { get; set; }
        public CancellationTokenSource? CancellationTokenSource { get; set; }
    }
}
