namespace Discounts
{
    public class MembershipDiscount : IDiscountStrategy
    {
        public float ApplyDiscount(float totalPrice)
        {
            return totalPrice * 0.85f;
        }
    }
}