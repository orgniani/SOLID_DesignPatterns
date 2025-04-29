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
            _uiHandler = new CashRegisterUIHandler(logText, priceText, customerName, employeeName);
            _orderProcessor = new OrderProcessor(_orderManager, _receiptManager, this, screenBlocker);

            _orderProcessor.OnLog += _uiHandler.SetLogText;
            _orderProcessor.OnPriceUpdate += _uiHandler.SetPriceText;
            _orderProcessor.OnOrderCompleted += _uiHandler.ResetInputs;

            SetupButtons();

            _uiHandler.SetLogText("");
            _uiHandler.SetPriceText("");

            receiptText.text = "";
            screenBlocker.SetActive(false);
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

            if (!priceText)
            {
                Debug.LogError($"{name}: {nameof(priceText)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }
    }
}
