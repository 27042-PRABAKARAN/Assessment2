using Boiler.Models;

namespace Boiler.View
{
    /// <summary>
    /// Display class is used for displaying output
    /// </summary>
    internal static class ConsolePrinter
    {
        private const int NotificationLength = 70;
        private static int _nextLine = 0;
        private const int MaxNotificationRows = 10;
        private static readonly object LockObject = new object();
        private const int LogRow = 15;


        public static void WriteLine(string message)
        {
            lock (LockObject)
            {
                Console.WriteLine(message);
            }
        }

        /// <summary>
        /// to print the message in red
        /// </summary>
        /// <param name="message">the message that has to be printed in red</param>
        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// to print the message in Green
        /// </summary>
        /// <param name="message">the message that has to be printed in Green</param>
        public static void Success(string message)
        {
            lock (LockObject)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                WriteLine(message);
                Console.ResetColor();
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
        public static void ClearMenu(int height)
        {
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i <= height; i++)
            {
                WriteLine(new string(' ', 30));
            }
            Console.SetCursorPosition(0, 0);
        }

        /// <summary>
        /// Waits for user input key and clears the log
        /// </summary>
        /// <param name="height"> number of logs printed</param>
        /// <param name="originalLeft"> original cursor position </param>
        /// <param name="originalTop"> original cursor position </param>
        public static void WaitAndClearLog(int height, int originalLeft, int originalTop)
        {
            WriteLine("Enter Any key to get back to menu");
            Console.ReadKey();
            Console.SetCursorPosition(0, LogRow);
            for (int i = 0; i < LogRow; i++)
            {
                WriteLine(new string(' ', Console.WindowWidth));
            }
            Console.SetCursorPosition(originalLeft, originalTop);
        }

        /// <summary>
        /// Prints Log Table
        /// </summary>
        /// <param name="logs"> Enumerable of logs to be printed </param>
        public static void PrintTable(IEnumerable<Log> logs)
        {
            int originalLeft = Console.CursorLeft;
            int originalTop = Console.CursorTop;
            Console.SetCursorPosition(0, LogRow);
            foreach (Log log in logs)
            {
                WriteLine($"Time : {log.TimeStamp}, Event Name : {log.Event}, Event Details : {log.EventData}");
            }
            WaitAndClearLog(logs.Count(), originalLeft, originalTop);
        }
    }
}
