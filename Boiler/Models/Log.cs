namespace Boiler.Models
{
    /// <summary>
    /// Log model to log data
    /// </summary>
    internal class Log
    {
        /// <summary>
        /// Constructor for the instance of Log
        /// </summary>
        /// <param name="timeStamp"> Time the data is logged</param>
        /// <param name="eventName"> The name of the Event </param>
        /// <param name="EventData"> The data happened in Event</param>
        public Log(DateTime timeStamp, string eventName, string EventData)
        {
            this.EventData = EventData;
            this.TimeStamp = timeStamp;
            this.Event = eventName;
        }

        /// <summary>
        /// Gets or Sets the Time the data is logged
        /// </summary>
        /// <value>
        /// Time the data is logged
        /// </value>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Gets or Sets the Name of the Event
        /// </summary>
        /// <value>
        /// Name of the Event
        /// </value>
        public string Event { get; set; }

        /// <summary>
        /// Gets or Sets the Data happened in event
        /// </summary>
        /// <value>
        /// Data happened in event
        /// </value>
        public string EventData { get; set; }
    }
}
