using Boiler.Models.Enums;
using Boiler.Services;

namespace Boiler.View
{
    internal class ConsoleView
    {
        private readonly BoilerServices _boilerServices;
        private readonly NotificationServices _notificationServices;
        public ConsoleView(BoilerServices boilerServices, NotificationServices notificationServices)
        {
            this._notificationServices = notificationServices;
            this._boilerServices = boilerServices;
            this._notificationServices.NotifyEvent += this.NotifyUser;
        }

        public void Menu()
        {
            BoilerOptions? option = default;
            do
            {
                ConsolePrinter.WriteLine(@"===================
1. Toggle Switch
2. Start Boiler Sequence.
3. Stop Boiler Sequence.
4. Simulate Error.
5. Reset Lock.
6. View Logs.
7. Exit.
===================");
                option = UserInput.ReadEnum<BoilerOptions>("Enter an option: ");
                switch (option)
                {
                    case BoilerOptions.ToggleSwitch:
                        {
                            this._boilerServices.ToggleSwitch();
                            break;
                        }
                    case BoilerOptions.StartBoiler:
                        {
                            Task.Run(() => this._boilerServices.StartSequence());
                            break;
                        }
                    case BoilerOptions.StopBoiler:
                        {
                            this._boilerServices.StopBoiler();
                            break;
                        }
                    case BoilerOptions.SimulateError:
                        {
                            this._boilerServices.SimulateBoilerError();
                            break;
                        }
                    case BoilerOptions.ResetLock:
                        {
                            this._boilerServices.ResetLock();
                            break;
                        }
                    case BoilerOptions.FetchLog:
                        {
                            this.FetchLog();
                            break;
                        }
                }
            }
            while (option != BoilerOptions.Exit);
        }
        public void FetchLog()
        {
            ConsolePrinter.PrintTable(this._boilerServices.FetchLog());
        }
        public void NotifyUser(string message)
        {
            ConsolePrinter.WriteLine(message);
        }
    }
}
