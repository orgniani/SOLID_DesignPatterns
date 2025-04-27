using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Discounts;
using Notifications;
using Orders;
using Factories;
using System.Collections;

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
        [SerializeField] private Button cancelOrderButton;

        [Header("Texts")]
        [SerializeField] private TMP_Text receiptText;
        [SerializeField] private TMP_Text priceText;
        [SerializeField] private TMP_Text logText;

        [Header("Input Fields")]
        [SerializeField] private TMP_InputField employeeName;
        [SerializeField] private TMP_InputField customerName;

        [Header("Blocker")]
        [SerializeField] private GameObject screenBlocker;

        [Header("Parameters")]
        [SerializeField] private float receiptTextDelay = 1f;

        private OrderManager _orderManager;
        private UIReceiptManager _receiptManager;

        private Customer _currentCustomer;
        private Employee _currentEmployee;

        private List<IOrder> _pendingOrders = new();

        private void Awake()
        {
            ValidateReferences();

            screenBlocker.SetActive(false);

            _orderManager = new OrderManager();
            _receiptManager = new UIReceiptManager(_orderManager, receiptText, receiptTextDelay);

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
            cancelOrderButton.onClick.AddListener(CancelOrder);
        }

        private void CreateCoffee() => AddOrder(new CoffeeFactory().CreateOrder());
        private void CreateTea() => AddOrder(new TeaFactory().CreateOrder());
        private void CreateCake() => AddOrder(new CakeFactory().CreateOrder());
        private void CreateCombo() => AddOrder(new ComboFactory().CreateOrder());

        private void ApplyCashDiscount()
        {
            TryApplyDiscount(new CashDiscount(), "Cash Discount");
        }

        private void ApplyMembershipDiscount()
        {
            TryApplyDiscount(new MembershipDiscount(), "Membership Discount");
        }

        private void ApplyVolumeDiscount()
        {
            TryApplyDiscount(new VolumeDiscount());
        }

        private void TryApplyDiscount(IDiscountStrategy discountStrategy, string discountName = null)
        {
            bool isVolumeDiscount = discountStrategy is VolumeDiscount;

            if (_orderManager.GetAppliedDiscount() != null && !isVolumeDiscount)
            {
                LogMessage($"Cannot apply {discountName}: another discount has already been applied.");
                return;
            }

            _orderManager.SetDiscountStrategy(discountStrategy);

            if (!string.IsNullOrEmpty(discountName))
                LogMessage($"{discountName} applied.");
        }

        private void ProcessOrder()
        {
            if (_pendingOrders.Count == 0)
            {
                LogMessage("No order to process.");
                return;
            }

            ApplyVolumeDiscount();
            CompositeOrder fullOrder = BuildCompositeOrder(_pendingOrders);

            _orderManager.Subscribe(_currentCustomer);
            _orderManager.Subscribe(_currentEmployee);

            StartCoroutine(ProcessOrderCoroutine(fullOrder));
        }

        private IEnumerator ProcessOrderCoroutine(IOrder fullOrder)
        {
            screenBlocker.SetActive(true);

            float finalPrice = _orderManager.GetFinalPrice(fullOrder);

            priceText.text = $"Total: ${finalPrice:F2}\n";

            yield return StartCoroutine(_receiptManager.BuildReceiptCoroutine(fullOrder));

            float preparationTime = fullOrder.GetPreparationTime();
            yield return StartCoroutine(PreparationCountdown(preparationTime));

            _orderManager.ProcessOrder(fullOrder);
            _pendingOrders.Clear();
            LogMessage("Order processed!");

            ResetInputField(customerName);
            ResetInputField(employeeName);

            screenBlocker.SetActive(false);
        }

        private IEnumerator PreparationCountdown(float duration)
        {
            float timer = duration;
            while (timer > 0)
            {
                LogMessage($"Preparing... {timer:F0} seconds left!");
                timer -= Time.deltaTime;
                yield return null;
            }
        }

        private void CancelOrder()
        {
            if (_pendingOrders.Count == 0)
            {
                LogMessage("No pending order to cancel.");
                return;
            }

            _pendingOrders.Clear();
            receiptText.text = "";
            priceText.text = "";

            ResetInputField(customerName);
            ResetInputField(employeeName);

            LogMessage("Pending order canceled.");
        }

        private void ResetInputField(TMP_InputField inputField)
        {
            inputField.text = string.Empty;
            inputField.placeholder.gameObject.SetActive(true);
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

        private void AddOrder(IOrder order)
        {
            if (_pendingOrders.Count == 0)
            {
                _currentCustomer = new Customer(customerName.text);
                _currentEmployee = new Employee(employeeName.text);
            }

            _pendingOrders.Add(order);
            UpdatePrices();
            LogMessage($"Added {UIOrderNameHelper.GetOrderName(order)}.");
        }

        private void UpdatePrices()
        {
            foreach (var order in _pendingOrders)
                priceText.text = $"{UIOrderNameHelper.GetOrderName(order)} ... ${order.GetPrice():F2}";
        }

        private void LogMessage(string message)
        {
            logText.text = message;
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

            if (!cancelOrderButton)
            {
                Debug.LogError($"{name}: {nameof(cancelOrderButton)} is null!" +
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
