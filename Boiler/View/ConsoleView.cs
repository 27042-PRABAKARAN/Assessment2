using Boiler.Models.Enums;
using Boiler.Services;

namespace Boiler.View
{
    /// <summary>
    /// User Interface to control Boiler
    /// </summary>
    internal class ConsoleView
    {
        private readonly IBoilerServices _boilerServices;
        private readonly NotificationServices _notificationServices;

        /// <summary>
        /// constructor to create instance of consoleView
        /// </summary>
        /// <param name="boilerServices"> instance of boiler services </param>
        /// <param name="notificationServices"> instance of notification services</param>
        public ConsoleView(IBoilerServices boilerServices, NotificationServices notificationServices)
        {
            this._notificationServices = notificationServices;
            this._boilerServices = boilerServices;
            this._notificationServices.NotifyEvent += this.NotifyUser;
            this._notificationServices.Execute("Boiler Control initialized");
        }

        /// <summary>
        /// To print Menu and Execute boiler Operations
        /// </summary>
        public void ExecuteOperations()
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
                            Task.Run(() => this._boilerServices.StartBoiler());
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
                    case BoilerOptions.DisplayLog:
                        {
                            this.FetchLog();
                            break;
                        }
                    case BoilerOptions.Exit:
                        {
                            this._boilerServices.StopBoiler();
                            break;
                        }
                    default:
                        {
                            ConsolePrinter.WriteLine("Enter valid Choice");
                            ConsolePrinter.WriteLine("Enter any key to return to menu");
                            Console.ReadKey();
                            break;
                        }
                }
                ConsolePrinter.ClearMenu();

            }
            while (option != BoilerOptions.Exit);
        }

        /// <summary>
        /// Fetches and Displays Logs
        /// </summary>
        public void FetchLog()
        {
            ConsolePrinter.PrintTable(this._boilerServices.FetchLog());
        }

        /// <summary>
        /// Notifies the user
        /// </summary>
        /// <param name="message"> the message to be notified</param>
        public void NotifyUser(string message)
        {
            ConsolePrinter.Notification(message);
        }
    }
}
