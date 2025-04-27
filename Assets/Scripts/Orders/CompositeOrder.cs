using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Orders
{
    public class CompositeOrder : IOrder
    {
        private List<IOrder> _orders = new();

        public void AddOrder(IOrder order)
        {
            _orders.Add(order);
        }

        public float GetPrice()
        {
            return _orders.Sum(order => order.GetPrice());
        }

        public float GetPreparationTime()
        {
            return _orders.Sum(order => order.GetPreparationTime());
        }

        public List<IOrder> GetOrders()
        {
            return _orders;
        }
    }
}