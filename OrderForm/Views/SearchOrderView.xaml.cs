using OrderForm.Helpers;
using OrderForm.Windows;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OrderForm.Views
{
    /// <summary>
    /// Interaction logic for SearchOrderView.xaml
    /// </summary>
    public partial class SearchOrderView : UserControl, IForm
    {
        public List<UIElement> formElements { get; set; }
        private MainWindow mainWindow;

        #region Constructor
        public SearchOrderView()
        {
            InitializeComponent();

            this.Loaded += ViewLoaded;

            formElements = new List<UIElement>();
            formElements.AddRange(FindFormElements(OrderForm));
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

        #region IForm Implementation
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
                FormResult.Text = "You must have at least one field filled to search for an order!";
                return false;
            }

            //If all fields are not blank, we can continue to check for errors.
            //Check id field
            #region orderField
            string orderField = FormTextBoxOrder.Text.Trim();
            if (orderField != String.Empty)
            {
                if (!orderField.All(Char.IsDigit))
                {
                    formValid = false;
                    FormIDError.Text = "This field contains invalid characters!";
                }
            }
            #endregion

            #region customerField
            string customerField = FormTextBoxOrder.Text.Trim();
            if (customerField != String.Empty)
            {
                if (!customerField.All(Char.IsDigit))
                {
                    formValid = false;
                    FormIDError.Text = "This field contains invalid characters!";
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
        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            if (CheckForErrors())
            {
                DatabaseConnection connection = mainWindow.databaseConnection;

                //Build a dictionary of values to check for in out SQL query
                Dictionary<string, string> queryValues = new Dictionary<string, string>();
                queryValues.Add("order_id", FormTextBoxOrder.Text.Trim());
                queryValues.Add("customer_id", FormTextBoxCustomer.Text.Trim());

                string queryString = DatabaseConnectionHelper.BuildSelectStatement("SELECT * FROM Orders WHERE ", queryValues);
                SqlDataReader sqlReader = connection.SQLReaderQuery(queryString);
                List<Order> orderList = DatabaseConnectionHelper.CreateOrderList(sqlReader);
                connection.closeConnection();
                DatabaseConnectionHelper.PopulateOrderList(orderList, connection);
                connection.closeConnection();

                OrderListWindow customerListWindow = new OrderListWindow(mainWindow, orderList);
                FormResult.Text = "Searching for order...   Found (" + orderList.Count + ") result(s)!";
            }
            else { FormResult.Text = "There are error(s) in your form! The order could not be searched!"; }
        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }
        #endregion
    }
}
