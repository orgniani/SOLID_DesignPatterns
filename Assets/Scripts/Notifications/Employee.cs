using UnityEngine;

namespace Notifications
{
    public class Employee : ISubscriber
    {
        private string _name = "Employee";

        public Employee(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return;
            _name = name;
        }

        public void Notify(string message)
        {
            Debug.Log($"{_name}: {message} Please deliver it to the customer.");
        }
    }
}