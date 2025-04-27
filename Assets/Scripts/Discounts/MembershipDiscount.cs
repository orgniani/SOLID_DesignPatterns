namespace Discounts
{
    public class MembershipDiscount : IDiscountStrategy
    {
        public string GetName()
        {
            return "Membership";
        }

        public float ApplyDiscount(float totalPrice)
        {
            return totalPrice * 0.85f;
        }
    }
}