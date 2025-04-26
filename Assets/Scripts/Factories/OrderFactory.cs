using Orders;

namespace Factories
{
    public abstract class OrderFactory
    {
        public abstract IOrder CreateOrder();
    }
}