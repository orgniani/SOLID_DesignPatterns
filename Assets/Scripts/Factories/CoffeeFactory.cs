using Orders;

namespace Factories
{
    public class CoffeeFactory : OrderFactory
    {
        public override IOrder CreateOrder()
        {
            return new SimpleOrder("Coffee", 5.0f);
        }
    }
}
