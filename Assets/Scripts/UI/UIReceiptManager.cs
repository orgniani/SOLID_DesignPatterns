using Orders;

namespace UI
{
    public class UIReceiptManager
    {
        public string BuildReceipt(IOrder order, OrderManager orderManager)
        {
            string receipt = "Receipt\n";
            float subtotal = 0f;

            if (order is CompositeOrder composite)
            {
                foreach (var item in composite.GetOrders())
                {
                    float price = item.GetPrice();
                    subtotal += price;
                    receipt += $"{UIOrderNameHelper.GetOrderName(item)} ... ${price:F2}\n";
                }
            }

            else
            {
                float price = order.GetPrice();
                subtotal += price;
                receipt += $"{UIOrderNameHelper.GetOrderName(order)} ... ${price:F2}\n";
            }

            receipt += "\n";

            float finalPrice = orderManager.GetFinalPrice(order);
            float discountAmount = subtotal - finalPrice;

            if (discountAmount > 0f && orderManager.GetAppliedDiscount() != null)
            {
                receipt += $"{orderManager.GetAppliedDiscount().GetName()} Discount Applied: -${discountAmount:F2}\n";
            }

            receipt += "-----\n";
            receipt += $"Total: ${finalPrice:F2}\n";

            return receipt;
        }
    }
}