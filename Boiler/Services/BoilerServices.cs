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

        public BoilerServices(BoilerSystem boilerSystem, LoggerServices loggerServices, NotificationServices notificationService)
        {
            this.boilerSystem = boilerSystem;
            this._logger = loggerServices;
            _notificationService = notificationService;
        }
        private BoilerSystem boilerSystem;

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
                    this._notificationService.Execute("the Boiler is stopped with error");
                    this._logger.Log(new Log(DateTime.Now, "Error", "the Boiler is stopped with error"));
                    this.boilerSystem.SystemStatus = SystemStatus.LockOut;
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
                this.boilerSystem.SwitchStatus = SwitchStatus.Open;
                this.boilerSystem.CancellationTokenSource?.Cancel();
                this._notificationService.Execute($"Switch is toggled to {this.boilerSystem.SwitchStatus}");
                this._logger.Log(new Log(DateTime.Now, "Information", $"The Switch is Toggled to state - {this.boilerSystem.SwitchStatus}"));
            }
        }
    }
}
