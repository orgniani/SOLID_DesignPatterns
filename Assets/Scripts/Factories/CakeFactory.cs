using Orders;

namespace Factories
{
    public class CakeFactory : OrderFactory
    {
        public override IOrder CreateOrder()
        {
            return new SimpleOrder("Cake", 6.5f);
        }
    }
}