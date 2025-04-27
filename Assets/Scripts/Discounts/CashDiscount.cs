namespace Discounts
{
    public class CashDiscount : IDiscountStrategy
    {
        public float ApplyDiscount(float totalPrice)
        {
            return totalPrice * 0.75f;
        }
    }
}
