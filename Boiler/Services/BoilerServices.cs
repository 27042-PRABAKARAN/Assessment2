using Boiler.Models;
using Boiler.Models.Enums;

namespace Boiler.Services
{
    internal class BoilerServices
    {
        private readonly LoggerServices _logger;
        private readonly NotificationServices _notificationService;
        private readonly TimeSpan _prePrudgeTime = TimeSpan.FromSeconds(10);
        private readonly TimeSpan _ignitionTime = TimeSpan.FromSeconds(10);
        private BoilerSystem boilerSystem;

        public BoilerServices(BoilerSystem boilerSystem, LoggerServices loggerServices, NotificationServices notificationService)
        {
            this.boilerSystem = boilerSystem;
            this._logger = loggerServices;
            _notificationService = notificationService;
        }

        public async Task StartSequence()
        {
            this.boilerSystem.CancellationTokenSource = new CancellationTokenSource();
            if (this.boilerSystem.SwitchStatus == SwitchStatus.Open)
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

        public void ToggleSwitch()
        {
            if (this.boilerSystem.SwitchStatus == SwitchStatus.Open)
            {
                this.boilerSystem.SwitchStatus = SwitchStatus.Close;
                this._notificationService.Execute($"Switch is toggled to {this.boilerSystem.SwitchStatus}");
                this._logger.Log(new Log(DateTime.Now, "Information", $"The Switch is Toggled to state - {this.boilerSystem.SwitchStatus}"));
            }
            else
            {
                this.StopBoiler();
                this.boilerSystem.SwitchStatus = SwitchStatus.Open;
                this.boilerSystem.SystemStatus = SystemStatus.LockOut;
            }
        }

        public void StopBoiler()
        {
            if (this.boilerSystem.SwitchStatus == SwitchStatus.Open)
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

        public void ResetLock()
        {
            if (this.boilerSystem.SwitchStatus == SwitchStatus.Close)
            {
                if ((this.boilerSystem.SystemStatus == SystemStatus.PrePrudge || this.boilerSystem.SystemStatus == SystemStatus.Ignition) && this.boilerSystem.SwitchStatus == SwitchStatus.Close)
                {
                    this.boilerSystem.CancellationTokenSource?.Cancel();
                }

                this.boilerSystem.SystemStatus = SystemStatus.Ready;
            }
            this._notificationService.Execute("the Boiler reset is complete");
            this._logger.Log(new Log(DateTime.Now, "Information", "the Boiler Reset is complete"));
        }

        public IEnumerable<Log> FetchLog()
        {
            return this._logger.FetchLog();
        }

    }
}
