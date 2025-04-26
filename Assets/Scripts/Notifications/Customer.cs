using UnityEngine;

namespace Notifications
{
    public class Customer : ISubscriber
    {
        public string Name { get; }

        public Customer(string name)
        {
            Name = name;
        }

        public void Notify(string message)
        {
            Debug.Log($"Customer {Name} received notification: {message}");
        }
    }
}