using UnityEngine;

namespace Orders
{
    public class SimpleOrder : IOrder
    {
        private float _price;
        public string Name { get; set; }

        public SimpleOrder(string name, float price)
        {
            Name = name;
            _price = price;
        }

        public float GetPrice()
        {
            return _price;
        }

        public void ShowDetails()
        {
            Debug.Log($"Simple Order: {Name} - ${_price}");
        }
    }
}