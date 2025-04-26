using Orders;

namespace Factories
{
    public class ComboFactory : OrderFactory
    {
        public override IOrder CreateOrder()
        {
            CompositeOrder combo = new CompositeOrder();
            combo.AddOrder(new CoffeeFactory().CreateOrder());
            combo.AddOrder(new CakeFactory().CreateOrder());
            return combo;
        }
    }
}