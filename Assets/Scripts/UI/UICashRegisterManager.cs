using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Discounts;
using Notifications;
using Orders;
using Factories;

namespace UI
{
    public class UICashRegisterManager : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button createCoffeeButton;
        [SerializeField] private Button createTeaButton;
        [SerializeField] private Button createCakeButton;
        [SerializeField] private Button createComboButton;
        [SerializeField] private Button applyCashDiscountButton;
        [SerializeField] private Button applyMembershipDiscountButton;
        [SerializeField] private Button processOrderButton;

        [Header("Texts")]
        [SerializeField] private TMP_Text receiptText;
        [SerializeField] private TMP_Text priceText;
        [SerializeField] private TMP_Text logText;

        private OrderManager _orderManager;
        private UIReceiptManager _receiptManager;

        private List<IOrder> _pendingOrders = new();

        private void Awake()
        {
            ValidateReferences();

            _orderManager = new OrderManager();
            _receiptManager = new UIReceiptManager();

            Customer customer = new Customer();
            Employee employee = new Employee();

            _orderManager.Subscribe(customer);
            _orderManager.Subscribe(employee);

            SetupButtons();

            receiptText.text = "";
            priceText.text = "";
            logText.text = "";

        }

        private void SetupButtons()
        {
            createCoffeeButton.onClick.AddListener(CreateCoffee);
            createTeaButton.onClick.AddListener(CreateTea);
            createCakeButton.onClick.AddListener(CreateCake);
            createComboButton.onClick.AddListener(CreateCombo);

            applyCashDiscountButton.onClick.AddListener(ApplyCashDiscount);
            applyMembershipDiscountButton.onClick.AddListener(ApplyMembershipDiscount);

            processOrderButton.onClick.AddListener(ProcessOrder);
        }

        private void CreateCoffee() => AddOrder(new CoffeeFactory().CreateOrder());
        private void CreateTea() => AddOrder(new TeaFactory().CreateOrder());
        private void CreateCake() => AddOrder(new CakeFactory().CreateOrder());
        private void CreateCombo() => AddOrder(new ComboFactory().CreateOrder());

        private void ApplyCashDiscount()
        {
            _orderManager.SetDiscountStrategy(new CashDiscount());
            LogMessage("Cash Discount applied.");
        }

        private void ApplyMembershipDiscount()
        {
            _orderManager.SetDiscountStrategy(new MembershipDiscount());
            LogMessage("Membership Discount applied.");
        }

        private void ProcessOrder()
        {
            if (_pendingOrders.Count == 0)
            {
                LogMessage("No orders to process.");
                return;
            }

            _orderManager.SetDiscountStrategy(new VolumeDiscount());

            CompositeOrder fullOrder = BuildCompositeOrder(_pendingOrders);
            string finalReceipt = _receiptManager.BuildReceipt(fullOrder, _orderManager);

            receiptText.text = finalReceipt;

            float finalPrice = _orderManager.GetFinalPrice(fullOrder);
            priceText.text = $"Total: ${finalPrice:F2}\n";

            _orderManager.ProcessOrder(fullOrder);
            _pendingOrders.Clear();
            LogMessage("Order processed!");
        }

        private CompositeOrder BuildCompositeOrder(List<IOrder> orders)
        {
            CompositeOrder compositeOrder = new CompositeOrder();
            foreach (var order in orders)
            {
                compositeOrder.AddOrder(order);
            }
            return compositeOrder;
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

        private void AddOrder(IOrder order)
        {
            _pendingOrders.Add(order);
            UpdateOrderList();
            LogMessage($"Added {GetOrderName(order)}.");
        }

        private void UpdateOrderList()
        {
            foreach (var order in _pendingOrders)
                priceText.text = $"{GetOrderName(order)} ... ${order.GetPrice():F2}";
        }

        private void LogMessage(string message)
        {
            logText.text = message;
            Debug.Log(message);
        }

        private void ValidateReferences()
        {
            if (!createCoffeeButton)
            {
                Debug.LogError($"{name}: {nameof(createCoffeeButton)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!createTeaButton)
            {
                Debug.LogError($"{name}: {nameof(createTeaButton)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!createCakeButton)
            {
                Debug.LogError($"{name}: {nameof(createCakeButton)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!createComboButton)
            {
                Debug.LogError($"{name}: {nameof(createComboButton)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!applyCashDiscountButton)
            {
                Debug.LogError($"{name}: {nameof(applyCashDiscountButton)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!applyMembershipDiscountButton)
            {
                Debug.LogError($"{name}: {nameof(applyMembershipDiscountButton)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!processOrderButton)
            {
                Debug.LogError($"{name}: {nameof(processOrderButton)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!receiptText)
            {
                Debug.LogError($"{name}: {nameof(receiptText)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            if (!logText)
            {
                Debug.LogError($"{name}: {nameof(logText)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }
    }
}
