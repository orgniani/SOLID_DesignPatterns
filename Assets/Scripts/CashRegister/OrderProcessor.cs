using Discounts;
using Managers;
using Notifications;
using Orders;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;

namespace CashRegister
{
    public class OrderProcessor
    {
        private readonly IOrderManager _orderManager;
        private readonly ReceiptManager _receiptManager;
        private readonly MonoBehaviour _coroutineHost;
        private readonly GameObject _screenBlocker;

        private readonly List<IOrder> _pendingOrders = new();
        private Customer _currentCustomer;
        private Employee _currentEmployee;

        public event Action<string> OnLog;
        public event Action<string> OnPriceUpdate;
        public event Action OnOrderCompleted;

        public OrderProcessor(
            IOrderManager orderManager,
            ReceiptManager receiptManager,
            MonoBehaviour coroutineHost,
            GameObject screenBlocker)
        {
            _orderManager = orderManager;
            _receiptManager = receiptManager;
            _coroutineHost = coroutineHost;
            _screenBlocker = screenBlocker;
        }

        public void AddOrder(IOrder order, Customer customer, Employee employee)
        {
            if (_pendingOrders.Count == 0)
            {
                _currentCustomer = customer;
                _currentEmployee = employee;
            }

            _pendingOrders.Add(order);
            UpdatePriceDisplay();
            OnLog?.Invoke("Added " + OrderNameHelper.GetOrderName(order));
        }

        public void CancelOrder()
        {
            if (_pendingOrders.Count == 0)
            {
                OnLog?.Invoke("No pending order to cancel.");
                return;
            }

            _pendingOrders.Clear();
            OnPriceUpdate?.Invoke("");
            OnLog?.Invoke("Pending order canceled.");
        }

        public void ApplyDiscount(IDiscountStrategy discount, string name = null)
        {
            bool isVolume = discount is VolumeDiscount;

            if (_orderManager.GetAppliedDiscount() != null)
            {
                if (!isVolume)
                    OnLog?.Invoke($"Cannot apply {name}: another discount has already been added");

                else
                    Debug.Log($"Cannot apply Volume Discount: another discount has already been added.");

                return;
            }
            _orderManager.SetDiscountStrategy(discount);

            if (!string.IsNullOrEmpty(name))
                OnLog?.Invoke($"{name} applied.");
        }

        public void ProcessOrder()
        {
            if (_pendingOrders.Count == 0)
            {
                OnLog?.Invoke("No order to process.");
                return;
            }

            ApplyDiscount(new VolumeDiscount());

            var fullOrder = BuildCompositeOrder();
            _orderManager.Subscribe(_currentCustomer);
            _orderManager.Subscribe(_currentEmployee);

            _coroutineHost.StartCoroutine(ProcessOrderCoroutine(fullOrder));
        }

        private IEnumerator ProcessOrderCoroutine(IOrder fullOrder)
        {
            _screenBlocker.SetActive(true);

            float finalPrice = _orderManager.GetFinalPrice(fullOrder);
            OnPriceUpdate?.Invoke($"Total: ${finalPrice:F2}\n");

            yield return _coroutineHost.StartCoroutine(_receiptManager.BuildReceiptCoroutine(fullOrder));

            float prepTime = fullOrder.GetPreparationTime();
            float timer = prepTime;

            while (timer > 0)
            {
                OnLog?.Invoke($"Preparing... {timer:F0} seconds left!");
                timer -= Time.deltaTime;
                yield return null;
            }

            _orderManager.ProcessOrder(fullOrder);
            _pendingOrders.Clear();

            OnLog?.Invoke("Order processed!");
            OnOrderCompleted?.Invoke();
            _screenBlocker.SetActive(false);
        }

        private CompositeOrder BuildCompositeOrder()
        {
            CompositeOrder composite = new CompositeOrder();

            foreach (var o in _pendingOrders)
                composite.AddOrder(o);

            return composite;
        }

        private void UpdatePriceDisplay()
        {
            string result = "";
            foreach (var order in _pendingOrders)
                result = $"{OrderNameHelper.GetOrderName(order)} ... ${order.GetPrice():F2}\n";

            OnPriceUpdate?.Invoke(result);
        }
    }
}
