using Notifications;
using System.Diagnostics;
using TMPro;
using UnityEngine.UI;

namespace CashRegister
{
    public class CashRegisterUIHandler
    {
        private readonly TMP_Text _logText;
        private readonly TMP_Text _priceText;

        private readonly TMP_InputField _customerInput;
        private readonly TMP_InputField _employeeInput;

        private readonly TMP_Text _customerNotifText;
        private readonly TMP_Text _employeeNotifText;

        public CashRegisterUIHandler(TMP_Text logText, TMP_Text priceText,
            TMP_InputField customerInput, TMP_InputField employeeInput, 
            TMP_Text customerNotifText, TMP_Text employeeNotifText)
        {
            _logText = logText;
            _priceText = priceText;

            _customerInput = customerInput;
            _employeeInput = employeeInput;

            _customerNotifText = customerNotifText;
            _employeeNotifText = employeeNotifText;
        }

        public void SetCustomerNotifText(string message)
        {
            if (_customerNotifText != null)
                _customerNotifText.text = message;
        }

        public void SetEmployeeNotifText(string message)
        {
            if (_employeeNotifText != null)
                _employeeNotifText.text = message;
        }


        public void SetupButton(Button button, UnityEngine.Events.UnityAction callback)
        {
            if (button != null)
                button.onClick.AddListener(callback);
        }

        public void SetLogText(string message)
        {
            if (_logText != null)
                _logText.text = message;
        }

        public void SetPriceText(string message)
        {
            if (_priceText != null)
                _priceText.text = message;
        }

        public void ResetInputs()
        {
            ResetInputField(_customerInput);
            ResetInputField(_employeeInput);
        }

        public Customer CreateCustomerFromInput() => SubscriberFactory.CreateCustomer(_customerInput.text);
        public Employee CreateEmployeeFromInput() => SubscriberFactory.CreateEmployee(_employeeInput.text);

        private void ResetInputField(TMP_InputField field)
        {
            if (field != null)
            {
                field.text = string.Empty;
                field.placeholder.gameObject.SetActive(true);
            }
        }
    }
}