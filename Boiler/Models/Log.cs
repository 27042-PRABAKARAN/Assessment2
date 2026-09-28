namespace Boiler.Models
{
    internal class Log
    {
        public Log(DateTime timeStamp, string eventName, string EventData)
        {
            this.EventData = EventData;
            this.TimeStamp = timeStamp;
            this.Event = eventName;
        }
        public DateTime TimeStamp { get; set; }
        public string Event { get; set; }

        public string EventData { get; set; }
    }
}
