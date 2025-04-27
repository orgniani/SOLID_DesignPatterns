namespace Discounts
{
    public class CashDiscount : IDiscountStrategy
    {
        public string GetName()
        {
            return "Cash";
        }

        public float ApplyDiscount(float totalPrice)
        {
            return totalPrice * 0.75f;
        }
    }
}
