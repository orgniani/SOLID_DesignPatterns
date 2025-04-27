using UnityEngine;

namespace Notifications
{
    public class Employee : ISubscriber
    {
        public void Notify(string message)
        {
            Debug.Log($"Employee received notification: {message}");
        }
    }
}