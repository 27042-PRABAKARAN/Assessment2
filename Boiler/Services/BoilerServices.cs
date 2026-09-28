using Boiler.Models;
using Boiler.Models.Enums;

namespace Boiler.Services
{
    /// <summary>
    /// Demonstrates the Services provided by the Boiler
    /// </summary>
    internal class BoilerServices
    {
        private readonly LoggerServices _logger;
        private readonly NotificationServices _notificationService;
        private readonly TimeSpan _prePrudgeTime = TimeSpan.FromSeconds(10);
        private readonly TimeSpan _ignitionTime = TimeSpan.FromSeconds(10);
        private BoilerSystem boilerSystem;

        /// <summary>
        /// To create an instance of Boiler Services
        /// </summary>
        /// <param name="boilerSystem"> To access the instance of boilerSystem</param>
        /// <param name="loggerServices"> To access the instance of logger Services</param>
        /// <param name="notificationService"> To access the instance of notification Services</param>
        public BoilerServices(BoilerSystem boilerSystem, LoggerServices loggerServices, NotificationServices notificationService)
        {
            this.boilerSystem = boilerSystem;
            this._logger = loggerServices;
            _notificationService = notificationService;
        }

        /// <summary>
        /// Starts the Boiler Sequence
        /// </summary>
        public async Task StartBoiler()
        {
            this.boilerSystem.CancellationTokenSource = new CancellationTokenSource();
            if (this.boilerSystem.InterLockSwitch == SwitchStatus.Open)
            {
                this._notificationService.Execute("Close the Switch to Start the Boiler");
                this._logger.Log(new Log(DateTime.Now, "Warning", "Close the Switch to Start the Boiler"));
            }
            else if (this.boilerSystem.SystemStatus != SystemStatus.LockOut && this.boilerSystem.SystemStatus != SystemStatus.Ready)
            {
                this._notificationService.Execute("The Boiler is Already Started");
                this._logger.Log(new Log(DateTime.Now, "Warning", "The Boiler is already Started"));
            }
            else
            {
                try
                {
                    this._notificationService.Execute("Pre Prudge Cycle Starts");
                    this.boilerSystem.SystemStatus = SystemStatus.PrePrudge;
                    await Task.Delay(this._prePrudgeTime, this.boilerSystem.CancellationTokenSource.Token);
                    this._notificationService.Execute("Ignition Cycle Starts");
                    this.boilerSystem.SystemStatus = SystemStatus.Ignition;
                    await Task.Delay(this._ignitionTime, this.boilerSystem.CancellationTokenSource.Token);
                    this._notificationService.Execute("Boiler is in operational state");
                    this.boilerSystem.SystemStatus = SystemStatus.Operational;
                }
                catch (OperationCanceledException)
                {
                    this.boilerSystem.SystemStatus = SystemStatus.LockOut;
                    this._notificationService.Execute("the Boiler is stopped with error");
                    this._logger.Log(new Log(DateTime.Now, "Error", "the Boiler is stopped with error"));
                    return;
                }
            }
        }

        /// <summary>
        /// Toggles the state of the switch.
        /// </summary>
        public void ToggleSwitch()
        {
            if (this.boilerSystem.InterLockSwitch == SwitchStatus.Open)
            {
                this.boilerSystem.InterLockSwitch = SwitchStatus.Close;
                this.boilerSystem.SystemStatus = SystemStatus.Ready;
                this._notificationService.Execute($"Switch is toggled to {this.boilerSystem.InterLockSwitch}");
                this._logger.Log(new Log(DateTime.Now, "Information", $"The Switch is Toggled to state - {this.boilerSystem.InterLockSwitch}"));
            }
            else
            {
                if (this.boilerSystem.SystemStatus == SystemStatus.Ignition || this.boilerSystem.SystemStatus == SystemStatus.PrePrudge)
                {
                    this.StopBoiler();
                }
                this.boilerSystem.SystemStatus = SystemStatus.LockOut;
                this.boilerSystem.InterLockSwitch = SwitchStatus.Open;
                this._notificationService.Execute($"Switch is toggled to {this.boilerSystem.InterLockSwitch}");
                this._logger.Log(new Log(DateTime.Now, "Information", $"The Switch is Toggled to state - {this.boilerSystem.InterLockSwitch}"));

            }
        }

        /// <summary>
        /// Stops the Boiler
        /// </summary>
        public void StopBoiler()
        {
            if (this.boilerSystem.InterLockSwitch == SwitchStatus.Open)
            {
                this._notificationService.Execute($"The Boiler is not started");
                this._logger.Log(new Log(DateTime.Now, "Warning", "Attempted to stop the boiler when it is not yet started"));
            }
            else if (this.boilerSystem.SystemStatus == SystemStatus.Ignition || this.boilerSystem.SystemStatus == SystemStatus.PrePrudge)
            {
                this.boilerSystem.CancellationTokenSource?.Cancel();
            }
            else
            {
                this.boilerSystem.SystemStatus = SystemStatus.Ready;
                this._notificationService.Execute($"The Boiler is Stopped");
                this._logger.Log(new Log(DateTime.Now, "Information", "The Boiler is Stopped"));
            }
        }

        /// <summary>
        /// Simulates a boiler error only if boiler is in operational state
        /// </summary>
        public void SimulateBoilerError()
        {
            if (this.boilerSystem.SystemStatus != SystemStatus.Operational)
            {
                this._notificationService.Execute($"the System should be in operational state");
                this._logger.Log(new Log(DateTime.Now, "Warning", "Attempted to stimulate error when the boiler is not in operational state"));
                return;
            }

            this._notificationService.Execute("the Boiler is stopped with error");
            this._logger.Log(new Log(DateTime.Now, "Error", "the Boiler is stopped with error"));
            this.boilerSystem.SystemStatus = SystemStatus.LockOut;
        }

        /// <summary>
        /// Resets the Inter Lock switch and system status
        /// </summary>
        public void ResetLock()
        {
            if (this.boilerSystem.InterLockSwitch == SwitchStatus.Close)
            {
                if ((this.boilerSystem.SystemStatus == SystemStatus.PrePrudge || this.boilerSystem.SystemStatus == SystemStatus.Ignition) && this.boilerSystem.InterLockSwitch == SwitchStatus.Close)
                {
                    this.boilerSystem.CancellationTokenSource?.Cancel();
                }

                this.boilerSystem.SystemStatus = SystemStatus.Ready;
            }
            this._notificationService.Execute("the Boiler reset is complete");
            this._logger.Log(new Log(DateTime.Now, "Information", "the Boiler Reset is complete"));
        }

        /// <summary>
        /// fetches log from logger
        /// </summary>
        /// <returns> Enumerable of Logs</returns>
        public IEnumerable<Log> FetchLog()
        {
            return this._logger.FetchLog();
        }
    }
}
