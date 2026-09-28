using Boiler.Models;
using Boiler.Models.Enums;
using Boiler.Services;
using Boiler.View;

namespace Boiler
{
    /// <summary>
    /// Entry point of Application
    /// </summary>
    internal class Program
    {
        private static void Main()
        {
            LoggerServices loggerServices = new("log.csv");
            NotificationServices notificationServices = new NotificationServices();
            BoilerSystem boilerSystem = new(SystemStatus.LockOut, SwitchStatus.Open);
            BoilerServices boilerServices = new(boilerSystem, loggerServices, notificationServices);
            ConsoleView consoleView = new ConsoleView(boilerServices, notificationServices);
            consoleView.ExecuteOperations();
        }
    }
}
