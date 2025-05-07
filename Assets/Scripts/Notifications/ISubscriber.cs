using System;

namespace Notifications
{
    public interface ISubscriber
    {
        void Notify(string message);
        Action<string> OnNotify { get; set; }
    }
}