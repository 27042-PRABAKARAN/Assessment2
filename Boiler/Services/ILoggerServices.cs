using Boiler.Models;

namespace Boiler.Services
{
    internal interface ILoggerServices
    {
        /// <summary>
        /// Logs the file in file location
        /// </summary>
        /// <param name="log"> object to be logged</param>
        internal void Log(Log log);

        /// <summary>
        /// Fetches the logs from file location
        /// </summary>
        /// <returns> Enumerable of Logs </returns>
        internal IEnumerable<Log> FetchLog();
    }
}
