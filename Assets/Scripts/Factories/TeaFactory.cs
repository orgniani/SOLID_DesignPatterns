using Orders;

namespace Factories
{
    public class TeaFactory : OrderFactory
    {
        public override IOrder CreateOrder()
        {
            return new SimpleOrder("Tea", 4.0f);
        }
    }
}
