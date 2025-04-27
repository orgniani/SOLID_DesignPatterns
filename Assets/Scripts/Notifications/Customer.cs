using UnityEngine;

namespace Notifications
{
    public class Customer : ISubscriber
    {
        private string _name = "Costumer";

        public Customer(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return;
            _name = name;
        }

        public void Notify(string message)
        {
            Debug.Log($"{_name}: {message}");
        }
    }
}