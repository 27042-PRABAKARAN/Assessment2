using Boiler.Models;

namespace Boiler.Services
{
    internal interface IBoilerServices
    {
        /// <summary>
        /// Starts the Boiler Sequence
        /// </summary>
        public Task StartBoiler();

        /// <summary>
        /// Toggles the state of the switch.
        /// </summary>
        public void ToggleSwitch();

        /// <summary>
        /// Stops the Boiler
        /// </summary>
        public void StopBoiler();

        /// <summary>
        /// Simulates a boiler error only if boiler is in operational state
        /// </summary>
        public void SimulateBoilerError();

        /// <summary>
        /// Resets the Inter Lock switch and system status
        /// </summary>
        public void ResetLock();

        /// <summary>
        /// fetches log from logger
        /// </summary>
        /// <returns> Enumerable of Logs</returns>
        public IEnumerable<Log> FetchLog();
    }
}
