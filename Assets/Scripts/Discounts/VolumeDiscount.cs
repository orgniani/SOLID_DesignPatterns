namespace Discounts
{
    public class VolumeDiscount : IDiscountStrategy
    {
        public string GetName()
        {
            return "Volume";
        }

        public float ApplyDiscount(float totalPrice)
        {
            return totalPrice >= 20.0f ? totalPrice * 0.9f : totalPrice;
        }
    }
}