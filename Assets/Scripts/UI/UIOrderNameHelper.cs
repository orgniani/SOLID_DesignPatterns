using Orders;

namespace UI
{
    public static class UIOrderNameHelper
    {
        public static string GetOrderName(IOrder order)
        {
            if (order is SimpleOrder simpleOrder)
            {
                return simpleOrder.Name;
            }

            else if (order is CompositeOrder)
            {
                return "Combo";
            }

            else
            {
                return "Unknown";
            }
        }
    }
}