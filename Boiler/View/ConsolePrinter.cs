using Boiler.Models;

namespace Boiler.View
{
    /// <summary>
    /// Printing class is used for displaying output
    /// </summary>
    internal static class ConsolePrinter
    {
        private const int NotificationLength = 70;
        private static int _nextLine = 0;
        private const int MaxNotificationRows = 10;
        private static readonly object LockObject = new object();
        private const int LogRow = 15;
        private const int MaxMenuHeight = 15;

        /// <summary>
        /// Lock the object and write into console
        /// </summary>
        /// <param name="message"></param>
        public static void WriteLine(string message)
        {
            lock (LockObject)
            {
                Console.WriteLine(message);
            }
        }

        /// <summary>
        /// Displays notification in the top-right corner dynamically.
        /// </summary>
        public static void Notification(string message)
        {
            int row;

            lock (LockObject)
            {
                int originalLeft = Console.CursorLeft;
                int originalTop = Console.CursorTop;
                row = _nextLine;
                _nextLine = (_nextLine + 1) % MaxNotificationRows;
                int rightCorner = Console.WindowWidth - NotificationLength;
                Console.SetCursorPosition(rightCorner, row);
                Console.Write(new string(' ', NotificationLength));
                Console.SetCursorPosition(rightCorner, row);
                Console.Write($"[{message}]");
                Console.SetCursorPosition(originalLeft, originalTop);
            }
            StartClearTimer(row);
        }

        /// <summary>
        /// Clears the notification
        /// </summary>
        /// <param name="row"> the row the notification is printed</param>
        private static void StartClearTimer(int row)
        {
            System.Timers.Timer timer = new System.Timers.Timer(4000);
            timer.AutoReset = false;
            timer.Elapsed += (sender, e) =>
            {
                lock (LockObject)
                {
                    int originalLeft = Console.CursorLeft;
                    int originalTop = Console.CursorTop;
                    int rightCorner = Console.WindowWidth - NotificationLength;
                    Console.SetCursorPosition(rightCorner, row);
                    Console.Write(new string(' ', NotificationLength));
                    Console.SetCursorPosition(originalLeft, originalTop);
                }
                timer.Dispose();
            };
            timer.Start();
        }

        /// <summary>
        /// Clears the menu
        /// </summary>
        /// <param name="height"></param>
        public static void ClearMenu()
        {
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i <= MaxMenuHeight; i++)
            {
                WriteLine(new string(' ', 30));
            }
            Console.SetCursorPosition(0, 0);
        }

        /// <summary>
        /// Prints Log Table
        /// </summary>
        /// <param name="logs"> Enumerable of logs to be printed </param>
        public static void PrintTable(IEnumerable<Log> logs)
        {
            Console.SetCursorPosition(0, LogRow);
            WriteLine("Last 10 Logs: ");
            foreach (Log log in logs)
            {
                WriteLine($"Time : {log.TimeStamp}, Event Name : {log.Event}, Event Details : {log.EventData}");
            }
            WriteLine("Enter Any key to get back to menu");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
