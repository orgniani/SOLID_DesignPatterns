using UnityEngine;

namespace Notifications
{
    public class Customer : ISubscriber
    {
        public void Notify(string message)
        {
            Debug.Log($"Customer received notification: {message}");
        }
    }
}