using UnityEngine;

namespace Notifications
{
    public class Employee : ISubscriber
    {
        public string Name { get; }

        public Employee(string name)
        {
            Name = name;
        }

        public void Notify(string message)
        {
            Debug.Log($"Employee {Name} received notification: {message}");
        }
    }
}