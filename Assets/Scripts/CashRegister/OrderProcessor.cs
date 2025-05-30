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

        public event Action<string> OnCustomerNotification;
        public event Action<string> OnEmployeeNotification;

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

                _currentCustomer.OnNotify = message => OnCustomerNotification?.Invoke(message);
                _currentEmployee.OnNotify = message => OnEmployeeNotification?.Invoke(message);
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

            _orderManager.SetDiscountStrategy(null);

            _pendingOrders.Clear();
            OnPriceUpdate?.Invoke("");
            OnLog?.Invoke("Pending order canceled.");
        }

        public void ApplyDiscount(IDiscountStrategy discount, string name = null)
        {
            if (_orderManager.GetAppliedDiscount() != null)
            {
                OnLog?.Invoke($"Cannot apply {name}: another discount has already been added");
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

            if (_orderManager.GetAppliedDiscount() == null) 
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
