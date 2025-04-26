using Orders;

namespace UI
{
    public class UIReceiptManager
    {
        public string BuildReceipt(IOrder order, OrderManager orderManager)
        {
            string receipt = "==== Coffee Shop Receipt ====\n";
            float subtotal = 0f;

            if (order is CompositeOrder composite)
            {
                foreach (var item in composite.GetOrders())
                {
                    float price = item.GetPrice();
                    subtotal += price;
                    receipt += $"{GetOrderName(item),-15} ${price:F2}\n";
                }
            }

            else
            {
                float price = order.GetPrice();
                subtotal += price;
                receipt += $"{GetOrderName(order),-15} ${price:F2}\n";
            }

            receipt += "\n";

            float finalPrice = orderManager.GetFinalPrice(order);
            float discountAmount = subtotal - finalPrice;

            if (discountAmount > 0f)
            {
                receipt += $"Discount Applied: -${discountAmount:F2}\n";
            }

            receipt += "-----------------------------\n";
            receipt += $"Total: ${finalPrice:F2}\n";

            return receipt;
        }

        private string GetOrderName(IOrder order)
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