namespace Discounts
{
    public interface IDiscountStrategy
    {
        string GetName();
        float ApplyDiscount(float totalPrice);
    }
}
