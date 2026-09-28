using Boiler.Models;

namespace Boiler.Services
{
    /// <summary>
    /// Provides Logging Services
    /// </summary>
    internal class LoggerServices : ILoggerServices
    {
        private readonly string _filePath;

        private readonly object _lockObject = new();

        private List<Log> _logs;

        private const int LogCount = 10;

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
                this._logs = new List<Log>();
            }
            else
            {
                this._logs = this.LoadLogs();
            }
        }

        /// <summary>
        /// Logs the file in file location
        /// </summary>
        /// <param name="log"> object to be logged</param>
        public void Log(Log log)
        {
            lock (this._lockObject)
            {
                File.AppendAllTextAsync(this._filePath, $"{log.TimeStamp},{log.Event},{log.EventData}\n");
                this._logs.Add(log);
            }
        }

        /// <summary>
        /// Fetches the logs
        /// </summary>
        /// <returns>Last 10 logs </returns>
        public IEnumerable<Log> FetchLog()
        {
            return this._logs.TakeLast(LogCount);
        }

        /// <summary>
        /// Loads the logs from file location
        /// </summary>
        /// <returns> List of logs</returns>
        public List<Log> LoadLogs()
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
