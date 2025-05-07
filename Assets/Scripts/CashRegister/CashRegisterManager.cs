using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Discounts;
using Notifications;
using Orders;
using Factories;
using Managers;

namespace CashRegister
{
    public class CashRegisterManager : MonoBehaviour
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

        [Header("Notification Texts")]
        [SerializeField] private TMP_Text customerNotifText;
        [SerializeField] private TMP_Text employeeNotifText;

        [Header("Input Fields")]
        [SerializeField] private TMP_InputField employeeName;
        [SerializeField] private TMP_InputField customerName;

        [Header("Blocker")]
        [SerializeField] private GameObject screenBlocker;

        [Header("Parameters")]
        [SerializeField] private float receiptTextDelay = 1f;

        private IOrderManager _orderManager;
        private ReceiptManager _receiptManager;
        private CashRegisterUIHandler _uiHandler;
        private OrderProcessor _orderProcessor;

        private void Awake()
        {
            ValidateReferences();

            _orderManager = new OrderManager();
            _receiptManager = new ReceiptManager(_orderManager, receiptText, receiptTextDelay);
            _uiHandler = new CashRegisterUIHandler(logText, priceText, customerName, employeeName, customerNotifText, employeeNotifText);
            _orderProcessor = new OrderProcessor(_orderManager, _receiptManager, this, screenBlocker);

            _orderProcessor.OnLog += _uiHandler.SetLogText;
            _orderProcessor.OnPriceUpdate += _uiHandler.SetPriceText;
            _orderProcessor.OnOrderCompleted += _uiHandler.ResetInputs;

            SetupButtons();

            _uiHandler.SetLogText("");
            _uiHandler.SetPriceText("");

            receiptText.text = "";
            screenBlocker.SetActive(false);

            _orderProcessor.OnCustomerNotification += _uiHandler.SetCustomerNotifText;
            _orderProcessor.OnEmployeeNotification += _uiHandler.SetEmployeeNotifText;
        }

        private void SetupButtons()
        {
            _uiHandler.SetupButton(createCoffeeButton, () => AddOrder(new CoffeeFactory().CreateOrder()));
            _uiHandler.SetupButton(createTeaButton, () => AddOrder(new TeaFactory().CreateOrder()));
            _uiHandler.SetupButton(createCakeButton, () => AddOrder(new CakeFactory().CreateOrder()));
            _uiHandler.SetupButton(createComboButton, () => AddOrder(new ComboFactory().CreateOrder()));

            _uiHandler.SetupButton(applyCashDiscountButton, () => _orderProcessor.ApplyDiscount(new CashDiscount(), "Cash Discount"));
            _uiHandler.SetupButton(applyMembershipDiscountButton, () => _orderProcessor.ApplyDiscount(new MembershipDiscount(), "Membership Discount"));

            _uiHandler.SetupButton(processOrderButton, _orderProcessor.ProcessOrder);
            _uiHandler.SetupButton(cancelOrderButton, _orderProcessor.CancelOrder);
        }

        private void AddOrder(IOrder order)
        {
            Customer customer = _uiHandler.CreateCustomerFromInput();
            Employee employee = _uiHandler.CreateEmployeeFromInput();
            _orderProcessor.AddOrder(order, customer, employee);
        }

        private void ValidateReferences()
        {
            if (!ValidateReference(createCoffeeButton, nameof(createCoffeeButton))) return;
            if (!ValidateReference(createTeaButton, nameof(createTeaButton))) return;
            if (!ValidateReference(createCakeButton, nameof(createCakeButton))) return;
            if (!ValidateReference(createComboButton, nameof(createComboButton))) return;

            if (!ValidateReference(applyCashDiscountButton, nameof(applyCashDiscountButton))) return;
            if (!ValidateReference(applyMembershipDiscountButton, nameof(applyMembershipDiscountButton))) return;
            if (!ValidateReference(processOrderButton, nameof(processOrderButton))) return;

            if (!ValidateReference(cancelOrderButton, nameof(cancelOrderButton))) return;

            if (!ValidateReference(receiptText, nameof(receiptText))) return;
            if (!ValidateReference(logText, nameof(logText))) return;
            if (!ValidateReference(priceText, nameof(priceText))) return;
            if (!ValidateReference(customerNotifText, nameof(customerNotifText))) return;
            if (!ValidateReference(employeeNotifText, nameof(employeeNotifText))) return;

            if (!ValidateReference(employeeName, nameof(employeeName))) return;
            if (!ValidateReference(customerName, nameof(customerName))) return;

            if (!ValidateReference(screenBlocker, nameof(screenBlocker))) return;
        }

        private bool ValidateReference(Object reference, string referenceName)
        {
            if (reference != null) return true;

            Debug.LogError($"{name}: {referenceName} is null!" +
                           $"\nDisabling component to avoid errors.");
            enabled = false;
            return false;
        }
    }
}
