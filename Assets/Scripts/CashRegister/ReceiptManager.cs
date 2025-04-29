using Orders;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Managers;

namespace CashRegister
{
    public class ReceiptManager
    {
        private IOrderManager _orderManager;

        private TMP_Text _receiptText;
        private float _delay;

        private RectTransform _receipt;

        public ReceiptManager(IOrderManager orderManager, TMP_Text text, float delay)
        {

            _orderManager = orderManager;
            _receiptText = text;
            _delay = delay;

            _receipt = _receiptText.rectTransform.parent.GetComponent<RectTransform>();
        }

        public IEnumerator BuildReceiptCoroutine(IOrder order)
        {
            float subtotal = 0f;

            _receiptText.text = "";

            AddLineToReceipt("- Receipt -");
            yield return new WaitForSeconds(_delay);

            foreach (var item in GetOrders(order))
            {
                float price = item.GetPrice();
                subtotal += price;

                AddLineToReceipt($"{OrderNameHelper.GetOrderName(item)} ... ${price:F2}");
                yield return new WaitForSeconds(_delay);
            }

            yield return new WaitForSeconds(_delay);

            float finalPrice = _orderManager.GetFinalPrice(order);
            float discountAmount = subtotal - finalPrice;

            if (discountAmount > 0f && _orderManager.GetAppliedDiscount() != null)
            {
                AddLineToReceipt($"\n{_orderManager.GetAppliedDiscount().GetName()} Discount Applied: -${discountAmount:F2}");
                yield return new WaitForSeconds(_delay);
            }


            else if (discountAmount <= 0f)
            {
                AddLineToReceipt($" \n No discount found: -${discountAmount:F2}");
            }
                AddLineToReceipt("\n -----");
            yield return new WaitForSeconds(_delay);

            AddLineToReceipt($"Total: ${finalPrice:F2}");
        }

        private void AddLineToReceipt(string line)
        {
            _receiptText.text += line + "\n";
            LayoutRebuilder.ForceRebuildLayoutImmediate(_receipt);
        }

        private IEnumerable<IOrder> GetOrders(IOrder order)
        {
            if (order is CompositeOrder composite)
                return composite.GetOrders();
            else
                return new List<IOrder> { order };
        }

    }
}