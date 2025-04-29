using Discounts;
using Notifications;
using Orders;

namespace Managers
{
    public interface IOrderManager
    {
        IDiscountStrategy GetAppliedDiscount();
        float GetFinalPrice(IOrder order);
        void ProcessOrder(IOrder order);
        void SetDiscountStrategy(IDiscountStrategy discountStrategy);
        void Subscribe(ISubscriber subscriber);
    }
}
