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
                }
            }
            while (option != BoilerOptions.Exit);
        }

        public void NotifyUser(string message)
        {
            ConsolePrinter.WriteLine(message);
        }
    }
}
