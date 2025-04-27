using Orders;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIReceiptManager
    {
        private OrderManager _orderManager;

        private TMP_Text _receiptText;
        private float _lineDelay;

        private RectTransform _receipt;

        public UIReceiptManager(OrderManager orderManager, TMP_Text receiptText, float lineDelay = 0.5f)
        {
            _orderManager = orderManager;

            _receiptText = receiptText;
            _lineDelay = lineDelay;

            _receipt = _receiptText.rectTransform.parent.GetComponent<RectTransform>();
        }

        public IEnumerator BuildReceiptCoroutine(IOrder order)
        {
            float subtotal = 0f;

            _receiptText.text = "";

            AddLineToReceipt("- Receipt -");
            yield return new WaitForSeconds(_lineDelay);

            foreach (var item in GetOrders(order))
            {
                float price = item.GetPrice();
                subtotal += price;

                AddLineToReceipt($"{UIOrderNameHelper.GetOrderName(item)} ... ${price:F2}");
                yield return new WaitForSeconds(_lineDelay);
            }

            yield return new WaitForSeconds(_lineDelay);

            float finalPrice = _orderManager.GetFinalPrice(order);
            float discountAmount = subtotal - finalPrice;

            if (discountAmount > 0f && _orderManager.GetAppliedDiscount() != null)
            {
                AddLineToReceipt($" \n {_orderManager.GetAppliedDiscount().GetName()} Discount Applied: -${discountAmount:F2}");
                yield return new WaitForSeconds(_lineDelay);
            }

            AddLineToReceipt("\n -----");
            yield return new WaitForSeconds(_lineDelay);

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