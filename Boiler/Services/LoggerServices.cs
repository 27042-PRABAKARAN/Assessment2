using Boiler.Models;

namespace Boiler.Services
{
    /// <summary>
    /// Provides Logging Services
    /// </summary>
    internal class LoggerServices
    {
        private readonly string _filePath;

        private readonly object _lockObject = new();

        /// <summary>
        /// to create an instance of LoggerServices
        /// </summary>
        /// <param name="filePath"> the file location of the logger</param>
        public LoggerServices(string filePath)
        {
            this._filePath = filePath;
            if (!File.Exists(this._filePath))
            {
                File.AppendAllTextAsync(this._filePath, $"EventTime , Event , EventData\n");
            }
        }

        /// <summary>
        /// Logs the file in file location
        /// </summary>
        /// <param name="log"> object to be logged</param>
        internal void Log(Log log)
        {
            lock (this._lockObject)
            {
                File.AppendAllTextAsync(this._filePath, $"{log.TimeStamp},{log.Event},{log.EventData}\n");
            }
        }

        /// <summary>
        /// Fetches the logs from file location
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<Log> FetchLog()
        {
            IEnumerable<string> text;
            lock (this._lockObject)
            {
                text = File.ReadLines(this._filePath);
            }
            List<Log> logs = new List<Log>();
            foreach (string line in text.Skip(1))
            {
                string[] items = line.Split(',');
                DateTime.TryParse(items[0], out DateTime timeStamp);
                Log log = new Log(timeStamp, items[1], items[2]);
                logs.Add(log);
            }
            return logs;
        }
    }
}
