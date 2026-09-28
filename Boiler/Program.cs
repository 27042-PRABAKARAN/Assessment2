using Boiler.Models;
using Boiler.Models.Enums;
using Boiler.Services;
using Boiler.View;

namespace Boiler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            LoggerServices loggerServices = new("log.csv");
            CancellationTokenSource cts = new CancellationTokenSource();
            NotificationServices notificationServices = new NotificationServices();
            BoilerSystem boilerSystem = new(SystemStatus.LockOut, SwitchStatus.Open);
            BoilerServices boilerServices = new(boilerSystem, loggerServices, notificationServices);
            ConsoleView consoleView = new ConsoleView(boilerServices, notificationServices);
            consoleView.Menu();
        }
    }
}
