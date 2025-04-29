using Notifications;
using UnityEngine;

namespace CashRegister
{
    public class SubscriberFactory
    {
        public static Customer CreateCustomer(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                name = "Customer";
            return new Customer(name);
        }

        public static Employee CreateEmployee(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                name = "Employee";
            return new Employee(name);
        }
    }
}
