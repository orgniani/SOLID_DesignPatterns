namespace Notifications
{
    public interface ISubscriber
    {
        void Notify(string message);
    }
}