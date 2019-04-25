using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using OrderForm.Helpers;

namespace OrderForm.Views
{
    /// <summary>
    /// Interaction logic for CreateCustomerView.xaml
    /// </summary>
    public partial class CreateCustomerView : UserControl, IForm
    {
        public List<UIElement> formElements { get; set; }
        private MainWindow mainWindow;

        #region Constructor
        public CreateCustomerView()
        {
            InitializeComponent();

            this.Loaded += ViewLoaded;

            formElements = new List<UIElement>();
            formElements.AddRange(FindFormElements(CustomerForm));
        }

        /// <summary>
        /// Used to get the window of this view once the form is loaded since trying this in the constructor will return null
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewLoaded(object sender, RoutedEventArgs e)
        {
            mainWindow = (MainWindow)Window.GetWindow(this);
        }
        #endregion

        #region IForm implementation
        public void ResetForm()
        {
            //Begin by resetting any errors in the form
            ResetFormErrors();

            //Then start resetting any other elements that contain the "InputElement" tag.
            foreach (UIElement uiElement in formElements)
            {
                //All form inputs are in the form of FrameworkElement, so we only need to check for those
                Type elementType = uiElement.GetType();
                if (elementType.IsSubclassOf(typeof(FrameworkElement)))
                {
                    FrameworkElement formElement = (FrameworkElement)uiElement;
                    if (FrameworkElementHelper.checkElementTag(formElement, "InputElement"))
                    {
                        var switchType = new Dictionary<Type, Action<UIElement>>
                        {
                            { typeof(TextBox), (UIElement element) => ((TextBox)element).Text = String.Empty },
                            { typeof(CheckBox), (UIElement element) => ((CheckBox)element).IsChecked = false }
                        };

                        switchType[elementType](uiElement);
                    }
                }
            }
        }

        public bool CheckForErrors()
        {
            ResetFormErrors();
            bool formValid = true;

            //Check name field
            #region nameField
            string nameField = FormTextBoxName.Text.Trim();
            if (String.IsNullOrEmpty(nameField))
            {
                formValid = false;
                FormNameError.Text = "This field cannot be empty!";
            }
            else if (!Regex.IsMatch(nameField, @"^[\p{L}\s-]+$"))
            {
                formValid = false;
                FormNameError.Text = "This field contains invalid characters!";
            }
            #endregion

            //Check phone field
            #region phoneField
            string phoneField = FormTextBoxPhone.Text.Trim();
            if (String.IsNullOrEmpty(phoneField))
            {
                formValid = false;
                FormPhoneError.Text = "This field cannot be empty!";
            }
            else if (!phoneField.All(Char.IsDigit))
            {
                formValid = false;
                FormPhoneError.Text = "This field contains invalid characters!";
            }
            else if (phoneField.Length < 10)
            {
                formValid = false;
                FormPhoneError.Text = "This field does not contain a valid phone number!";
            }
            #endregion

            //Check email field
            #region emailField
            string emailField = FormTextBoxEmail.Text.Trim();
            if (String.IsNullOrEmpty(emailField))
            {
                formValid = false;
                FormEmailError.Text = "This field cannot be empty!";
            }
            else if (!Regex.IsMatch(emailField, @"^[\w\.\-\+]+@\w+-*\w+.\w+$"))
            {
                formValid = false;
                FormEmailError.Text = "This field does not contain a valid email address!";
            }
            #endregion

            //Check address field
            #region addressField
            string addressField = FormTextBoxAddress.Text.Trim();
            if (String.IsNullOrEmpty(addressField))
            {
                formValid = false;
                FormAddressError.Text = "This field cannot be empty!";
            }
            else if (!Regex.IsMatch(emailField, @"^[\w\s\p{P}][^;]+$"))
            {
                formValid = false;
                FormAddressError.Text = "This field does not contain a valid address!";
            }
            #endregion

            //Check city field
            #region cityField
            string cityField = FormTextBoxCity.Text.Trim();
            if (String.IsNullOrEmpty(cityField))
            {
                formValid = false;
                FormCityError.Text = "This field cannot be empty!";
            }
            else if (!Regex.IsMatch(cityField, @"^[\w\s\p{P}][^;]+$"))
            {
                formValid = false;
                FormCityError.Text = "This field does not contain a valid city!";
            }
            #endregion

            //Check state field
            #region stateField
            string stateField = FormTextBoxState.Text.Trim();
            if (String.IsNullOrEmpty(stateField))
            {
                formValid = false;
                FormStateError.Text = "This field cannot be empty!";
            }
            else if (!Regex.IsMatch(stateField, @"^[\p{L}]+$"))
            {
                formValid = false;
                FormStateError.Text = "This field contains invalid characters!";
            }
            #endregion

            //Check zip field
            #region zipField
            string zipField = FormTextBoxZip.Text.Trim();
            if (String.IsNullOrEmpty(zipField))
            {
                formValid = false;
                FormZipError.Text = "This field cannot be empty!";
            }
            else if (!zipField.All(Char.IsDigit))
            {
                formValid = false;
                FormZipError.Text = "This field contains invalid characters!";
            }
            else if (zipField.Length < 5)
            {
                formValid = false;
                FormPhoneError.Text = "This field does not contain a valid zip number!";
            }
            #endregion

            return formValid;
        }

        public void ResetFormErrors()
        {
            foreach (UIElement uiElement in formElements)
            {
                //All form errors are in the form of TextBlock, so we only need to check for those
                if (uiElement.GetType() == typeof(TextBlock))
                {
                    TextBlock formText = (TextBlock)uiElement;
                    if (FrameworkElementHelper.checkElementTag(formText, "ErrorBlock"))
                    {
                        //If everything checks out, and we know this is a ErrorBlock, reset the text to an empty string.
                        formText.Text = String.Empty;
                    }
                }
            }
        }

        public List<UIElement> FindFormElements(Panel panel)
        {
            return FormHelper.FindChildren(panel);
        }
        #endregion

        #region Click Events
        /// <summary>
        /// Resets the customer creation form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            if (CheckForErrors())
            {
                string queryString = String.Format("INSERT INTO RegisteredCustomers (customer_name, customer_phone, customer_email, customer_address, customer_city, customer_state," +
                " customer_zip) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}'); SELECT SCOPE_IDENTITY();", FormTextBoxName.Text.Trim(), FormTextBoxPhone.Text.Trim(), FormTextBoxEmail.Text.Trim(),
                FormTextBoxAddress.Text.Trim(), FormTextBoxCity.Text.Trim(), FormTextBoxState.Text.Trim(), FormTextBoxZip.Text.Trim());

                object sqlID = mainWindow.databaseConnection.SQLScalarQuery(queryString);
                int customerID = Convert.ToInt32(sqlID);

                FormResult.Text = "Customer has been created!   Customer ID: " + customerID;
            }
            else { FormResult.Text = "There are error(s) in your form! The customer could not be created!"; }
        }
        #endregion
    }
}
