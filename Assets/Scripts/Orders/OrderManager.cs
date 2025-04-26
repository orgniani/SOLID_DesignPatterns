using System.Collections.Generic;
using UnityEngine;
using Notifications;
using Discounts;

namespace Orders
{
    public class OrderManager
    {
        private List<ISubscriber> _subscribers = new();
        private IDiscountStrategy _discountStrategy;

        public void Subscribe(ISubscriber subscriber)
        {
            _subscribers.Add(subscriber);
        }

        public void SetDiscountStrategy(IDiscountStrategy discountStrategy)
        {
            _discountStrategy = discountStrategy;
        }

        public void ProcessOrder(IOrder order)
        {
            float totalPrice = order.GetPrice();
            float finalPrice = _discountStrategy != null ? _discountStrategy.ApplyDiscount(totalPrice) : totalPrice;

            order.ShowDetails();
            Debug.Log($"Final Price after discount: ${finalPrice}");

            NotifyAll("Your order is ready!");
        }

        public float GetFinalPrice(IOrder order)
        {
            float totalPrice = order.GetPrice();
            return _discountStrategy != null ? _discountStrategy.ApplyDiscount(totalPrice) : totalPrice;
        }

        private void NotifyAll(string message)
        {
            foreach (var subscriber in _subscribers)
            {
                subscriber.Notify(message);
            }
        }
    }
}