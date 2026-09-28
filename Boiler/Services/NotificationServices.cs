namespace Boiler.Services
{
    internal class NotificationServices
    {
        public delegate void Notify(string Message);
        public event Notify? NotifyEvent;

        public void Execute(string message)
        {
            NotifyEvent?.Invoke(message);
        }
    }
}
