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
    }
}
