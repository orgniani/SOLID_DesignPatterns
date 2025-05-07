using UnityEngine;

namespace Orders
{
    public class SimpleOrder : IOrder
    {
        private float _price;
        public string Name { get; private set; }

        public SimpleOrder(string name, float price)
        {
            Name = name;
            _price = price;
        }

        public float GetPrice() => _price;
    }
}