using System;
using UnityEngine;

namespace Notifications
{
    public class Customer : ISubscriber
    {
        private string _name = "Costumer";

        public Action<string> OnNotify { get; set; }

        public Customer(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return;
            _name = name;
        }

        public void Notify(string message)
        {
            string fullMessage = $"{_name}: {message} You can pick it up now.";

            Debug.Log(fullMessage);
            OnNotify?.Invoke(fullMessage);
        }
    }
}