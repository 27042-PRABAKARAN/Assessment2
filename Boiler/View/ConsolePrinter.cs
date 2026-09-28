using Boiler.Models;
namespace Boiler.View
{
    internal static class ConsolePrinter
    {
        public static void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public static void PrintTable(IEnumerable<Log> logs)
        {
            foreach (Log log in logs)
            {
                WriteLine($"Time : {log.TimeStamp}, Event Name : {log.Event}, Event Details : {log.EventData}");
            }
        }
    }
}
