using Boiler.Models;

namespace Boiler.Services
{
    internal class LoggerServices
    {
        private readonly string _filePath;

        private readonly object _lockObject = new();

        public LoggerServices(string filePath)
        {
            this._filePath = filePath;
            if (!File.Exists(this._filePath))
            {
                File.AppendAllTextAsync(this._filePath, $"EventTime , Event , EventData\n");
            }
        }

        internal void Log(Log log)
        {
            lock (this._lockObject)
            {
                File.AppendAllTextAsync(this._filePath, $"{log.TimeStamp},{log.Event},{log.EventData}\n");
            }
        }

        internal IEnumerable<Log> FetchLog()
        {
            IEnumerable<string> text = File.ReadLines(this._filePath);
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
