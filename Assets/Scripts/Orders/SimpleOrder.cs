using UnityEngine;

namespace Orders
{
    public class SimpleOrder : IOrder
    {
        private float _price;
        private float _preparationTime;
        public string Name { get; private set; }

        public SimpleOrder(string name, float price, float preparationTime = 1f)
        {
            Name = name;
            _price = price;
            _preparationTime = preparationTime;
        }

        public float GetPrice() => _price;
        public float GetPreparationTime() => _preparationTime;
    }
}