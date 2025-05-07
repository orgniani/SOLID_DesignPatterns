using System;
using UnityEngine;

namespace Notifications
{
    public class Employee : ISubscriber
    {
        private string _name = "Employee";

        public Action<string> OnNotify { get; set; }

        public Employee(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return;
            _name = name;
        }

        public void Notify(string message)
        {
            string fullMessage = $"{_name}: {message} Please deliver it to the customer.";

            Debug.Log(fullMessage);
            OnNotify?.Invoke(fullMessage);
        }
    }
}