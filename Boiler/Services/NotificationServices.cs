namespace Boiler.Services
{
    internal class NotificationServices
    {
        /// <summary>
        /// Signature of the method group to be subscribed
        /// </summary>
        /// <param name="Message"> message to be notified </param>
        public delegate void Notify(string Message);

        /// <summary>
        /// Event to trigger notification.
        /// </summary>
        public event Notify? NotifyEvent;

        /// <summary>
        /// Invokes the Event - NotifyEvent
        /// </summary>
        /// <param name="message"> the message to be notified</param>
        public void Execute(string message)
        {
            NotifyEvent?.Invoke(message);
        }
    }
}
