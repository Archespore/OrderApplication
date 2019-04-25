using OrderForm.Helpers;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Linq;
using OrderForm.Windows;

namespace OrderForm.Views
{
    /// <summary>
    /// Interaction logic for SearchCustomerView.xaml
    /// </summary>
    public partial class SearchCustomerView : UserControl, IForm
    {
        public List<UIElement> formElements { get; set; }
        private MainWindow mainWindow;

        #region Constructor
        public SearchCustomerView()
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
        public bool CheckForErrors()
        {
            ResetFormErrors();
            bool formBlank = true;
            bool formValid = true;

            //First make sure that all input elements are not blank
            foreach (UIElement uiElement in formElements)
            {
                //All form inputs are in the form of FrameworkElement, so we only need to check for those
                Type elementType = uiElement.GetType();
                if (elementType.IsSubclassOf(typeof(FrameworkElement)))
                {
                    FrameworkElement formElement = (FrameworkElement)uiElement;
                    if (FrameworkElementHelper.checkElementTag(formElement, "InputElement"))
                    {
                        var switchType = new Dictionary<Type, Func<UIElement, bool>>
                        {
                            { typeof(TextBox), (UIElement element) => ((TextBox)element).Text == String.Empty },
                            { typeof(ComboBox), (UIElement element) => ((ComboBox)element).SelectedIndex == -1 }
                        };

                        formBlank = (formBlank && switchType[elementType](uiElement));
                    }
                }
            }
            if (formBlank)
            {
                FormResult.Text = "You must have at least one field filled to search for a customer!";
                return false;
            }

            //If all fields are not blank, we can continue to check for errors.
            //Check id field
            #region idField
            string idField = FormTextBoxID.Text.Trim();
            if (idField != String.Empty)
            {
                if (!idField.All(Char.IsDigit))
                {
                    formValid = false;
                    FormIDError.Text = "This field contains invalid characters!";
                }
            }
            #endregion

            //Check name field
            #region nameField
            string nameField = FormTextBoxName.Text.Trim();
            if (nameField != String.Empty)
            {
                if (!Regex.IsMatch(nameField, @"^[\p{L}\s-]+$"))
                {
                    formValid = false;
                    FormNameError.Text = "This field contains invalid characters!";
                }
            }
            #endregion

            //Check phone field
            #region phoneField
            string phoneField = FormTextBoxPhone.Text.Trim();
            if (phoneField != String.Empty)
            {
                if (!phoneField.All(Char.IsDigit))
                {
                    formValid = false;
                    FormPhoneError.Text = "This field contains invalid characters!";
                }
                else if (phoneField.Length < 10)
                {
                    formValid = false;
                    FormPhoneError.Text = "This field does not contain a valid phone number!";
                }
            }
            #endregion

            //Check email field
            #region emailField
            string emailField = FormTextBoxEmail.Text.Trim();
            if (emailField != String.Empty)
            {
                if (!Regex.IsMatch(emailField, @"^[\w\.\-\+]+@\w+-*\w+\.\w+$"))
                {
                    formValid = false;
                    FormEmailError.Text = "This field does not contain a valid email address!";
                }
            }
            #endregion

            //Check address field
            #region addressField
            string addressField = FormTextBoxAddress.Text.Trim();
            if (addressField != String.Empty)
            {
                if (!Regex.IsMatch(emailField, @"^[\w\s\p{P}][^;]+$"))
                {
                    formValid = false;
                    FormAddressError.Text = "This field does not contain a valid address!";
                }
            }
            #endregion

            //Check city field
            #region cityField
            string cityField = FormTextBoxCity.Text.Trim();
            if (cityField != String.Empty)
            {
                if (!Regex.IsMatch(emailField, @"^[\w\s\p{P}][^;]+$"))
                {
                    formValid = false;
                    FormCityError.Text = "This field does not contain a valid city!";
                }
            }
            #endregion

            //Check state field
            #region stateField
            string stateField = FormTextBoxState.Text.Trim();
            if (stateField != String.Empty)
            {
                if (!Regex.IsMatch(stateField, @"^[\p{L}]+$"))
                {
                    formValid = false;
                    FormStateError.Text = "This field contains invalid characters!";
                }
            }
            #endregion

            //Check zip field
            #region zipField
            string zipField = FormTextBoxZip.Text.Trim();
            if (zipField != String.Empty)
            {
                if (!zipField.All(Char.IsDigit))
                {
                    formValid = false;
                    FormZipError.Text = "This field contains invalid characters!";
                }
                else if (zipField.Length < 5)
                {
                    formValid = false;
                    FormPhoneError.Text = "This field does not contain a valid zip number!";
                }
            }
            #endregion

            return formValid;
        }

        public List<UIElement> FindFormElements(Panel panel)
        {
            return FormHelper.FindChildren(panel);
        }

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
        #endregion

        #region Click Events
        /// <summary>
        /// Resets the customer search form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            if (CheckForErrors())
            {
                //Build a dictionary of values to check for in out SQL query
                Dictionary<string, string> queryValues = new Dictionary<string, string>();
                queryValues.Add("customer_id", FormTextBoxID.Text.Trim());
                queryValues.Add("customer_name", FormTextBoxName.Text.Trim());
                queryValues.Add("customer_phone", FormTextBoxPhone.Text.Trim());
                queryValues.Add("customer_email", FormTextBoxEmail.Text.Trim());
                queryValues.Add("customer_address", FormTextBoxAddress.Text.Trim());
                queryValues.Add("customer_city", FormTextBoxCity.Text.Trim());
                queryValues.Add("customer_state", FormTextBoxState.Text.Trim());
                queryValues.Add("customer_zip", FormTextBoxZip.Text.Trim());

                string queryString = DatabaseConnectionHelper.BuildSelectStatement("SELECT * FROM RegisteredCustomers WHERE ", queryValues);
                SqlDataReader sqlReader = mainWindow.databaseConnection.SQLReaderQuery(queryString);
                List<Customer> customerList = DatabaseConnectionHelper.CreateCustomerList(sqlReader);
                mainWindow.databaseConnection.closeConnection();

                CustomerListWindow customerListWindow = new CustomerListWindow(mainWindow, customerList);
                FormResult.Text = "Searching for customer...   Found (" + customerList.Count + ") result(s)!";
            }
            else { FormResult.Text = "There are error(s) in your form! The customer could not be searched!"; }
        }
        #endregion
    }
}
