namespace Discounts
{
    public interface IDiscountStrategy
    {
        float ApplyDiscount(float totalPrice);
    }
}
