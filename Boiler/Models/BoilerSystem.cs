using Boiler.Models.Enums;

namespace Boiler.Models
{
    /// <summary>
    /// The Core Boiler Model
    /// </summary>
    internal class BoilerSystem
    {
        /// <summary>
        /// Constructor for the instance of BoilerSystem
        /// </summary>
        /// <param name="systemStatus"> Status of the system </param>
        /// <param name="switchStatus"> status of the switch </param>
        public BoilerSystem(SystemStatus systemStatus, SwitchStatus switchStatus)
        {
            this.InterLockSwitch = switchStatus;
            this.SystemStatus = systemStatus;
        }

        /// <summary>
        /// Gets or Sets the Status of the System
        /// </summary>
        /// <value>
        /// Status of the System
        /// </value>
        public SystemStatus SystemStatus { get; set; }

        /// <summary>
        /// Gets or Sets the Status of the Switch
        /// </summary>
        /// <value>
        /// Status of the Switch
        /// </value>
        public SwitchStatus InterLockSwitch { get; set; }

        /// <summary>
        /// Gets or Sets the CancellationTokenSource
        /// </summary>
        /// <value>
        /// Status of the CancellationTokenSource
        /// </value>
        public CancellationTokenSource? CancellationTokenSource { get; set; }
    }
}
