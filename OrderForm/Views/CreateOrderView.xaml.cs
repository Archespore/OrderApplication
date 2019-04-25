using OrderForm.Helpers;
using OrderForm.Windows;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Security.Principal;

namespace OrderForm.Views
{
    /// <summary>
    /// Interaction logic for CreateOrderView.xaml
    /// </summary>
    public partial class CreateOrderView : UserControl, IForm
    {
        public List<UIElement> formElements { get; set; }
        private MainWindow mainWindow;

        #region Constructor
        public CreateOrderView()
        {
            InitializeComponent();

            this.Loaded += ViewLoaded;

            formElements = new List<UIElement>();
            formElements.AddRange(FindFormElements(OrderForm));
        }

        private void ViewLoaded(object sender, RoutedEventArgs e)
        {
            mainWindow = (MainWindow)Window.GetWindow(this);
        }
        #endregion

        #region IForm Implementation
        public bool CheckForErrors()
        {
            ResetFormErrors();
            bool formValid = true;

            //Check id field
            #region idField
            string idField = FormTextBoxID.Text.Trim();
            if (idField == String.Empty)
            {
                formValid = false;
                FormIDError.Text = "This field cannot be empty!";
            }
            else if (!idField.All(Char.IsDigit))
            {
                formValid = false;
                FormIDError.Text = "This field contains invalid characters!";
            }
            else
            {
                string queryString = String.Format("SELECT customer_id FROM RegisteredCustomers WHERE customer_id={0}", idField);
                object sqlID = mainWindow.databaseConnection.SQLScalarQuery(queryString);
                if (sqlID == null)
                {
                    formValid = false;
                    FormIDError.Text = "This customer ID does not exist in the database!";
                }
            }
            #endregion

            //Check fulfillment
            #region fulfillmentField
            bool? deliveryRadio = FormRadioDelivery.IsChecked;
            bool? pickupRadio = FormRadioPickup.IsChecked;
            if (deliveryRadio == false && pickupRadio == false)
            {
                formValid = false;
                FormFulfullmentError.Text = "This field cannot be empty!";
            }
            #endregion

            //Check notes
            #region notesField
            string notesField = FormTextBoxNotes.Text.Trim();
            if (notesField != String.Empty)
            {
                if (!Regex.IsMatch(notesField, @"^[\w\s\p{P}][^;]+$"))
                {
                    formValid = false;
                    FormNotesError.Text = "This field contains invalid characters!";
                }
            }
            #endregion

            //Check to make sure order isn't blank
            #region Order Checking
            Order currentOrder = ((CreateOrderTab)mainWindow.ActiveTabControl.SelectedItem).order;
            if (currentOrder.orderItems.Count <= 0)
            {
                formValid = false;
                FormResult.Text = "You cannot place an order with no items!";
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

            //Update the price box and clear all items in the order.
            FormTextBoxPrice.Text = "$0";
            ((CreateOrderTab)mainWindow.ActiveTabControl.SelectedItem).order.clearOrder();

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
                            { typeof(RadioButton), (UIElement element) => ((RadioButton)element).IsChecked = false }
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
        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            if (CheckForErrors())
            {
                Order currentOrder = ((CreateOrderTab)mainWindow.ActiveTabControl.SelectedItem).order;
                FormTextBoxPrice.Text = String.Concat("$", Convert.ToString(currentOrder.orderPrice));

                if (MessageBox.Show("Are you sure you wish to create this order?", "Order Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Question).Equals(MessageBoxResult.OK)) {
                    //Create the order SQL entry
                    bool fulfillmentString = FormRadioDelivery.IsChecked == true ? true : false;
                    string queryString = String.Format("INSERT INTO Orders (customer_id, order_creator, order_total, order_fulfillment, order_notes)" +
                        "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}'); SELECT SCOPE_IDENTITY();", FormTextBoxID.Text.Trim(), WindowsIdentity.GetCurrent().Name, currentOrder.orderPrice,
                        fulfillmentString, FormTextBoxNotes.Text.Trim());
                    object sqlID = mainWindow.databaseConnection.SQLScalarQuery(queryString);
                    int orderID = Convert.ToInt32(sqlID);

                    //Create the orderline SQL entries
                    int itemCount = 0;
                    foreach (MenuItem item in currentOrder.orderItems)
                    {
                        queryString = String.Format("INSERT INTO OrderLines (order_item_id, order_id, menu_item_id, order_item_modifiers)" +
                            "VALUES ('{0}', '{1}', '{2}', '{3}'); SELECT SCOPE_IDENTITY();", itemCount, orderID, item.menuItemID, item.ConvertModifiersToString());
                        mainWindow.databaseConnection.SQLNonQuery(queryString);
                        itemCount++;
                    }

                    ResetForm();
                    FormResult.Text = "Order has been created!   Order ID: " + orderID;
                }
            }
            else { FormResult.Text = "There are error(s) in your order! Order could not be created!"; }
        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }

        private void AddMainItemButton_Click(object sender, RoutedEventArgs e)
        {
            Order currentOrder = ((CreateOrderTab)mainWindow.ActiveTabControl.SelectedItem).order;
            ItemListWindow customerListWindow = new ItemListWindow(mainWindow, ItemHelper.getAllItems(DatabaseConnectionHelper.defaultConnection,
                ItemGroup.Main).Values.ToList<MenuItem>(), currentOrder);
        }

        private void AddSideItemButton_Click(object sender, RoutedEventArgs e)
        {
            Order currentOrder = ((CreateOrderTab)mainWindow.ActiveTabControl.SelectedItem).order;
            ItemListWindow customerListWindow = new ItemListWindow(mainWindow, ItemHelper.getAllItems(DatabaseConnectionHelper.defaultConnection,
                ItemGroup.Side).Values.ToList<MenuItem>(), currentOrder);
        }

        private void AddDrinkItemButton_Click(object sender, RoutedEventArgs e)
        {
            Order currentOrder = ((CreateOrderTab)mainWindow.ActiveTabControl.SelectedItem).order;
            ItemListWindow customerListWindow = new ItemListWindow(mainWindow, ItemHelper.getAllItems(DatabaseConnectionHelper.defaultConnection,
                ItemGroup.Beverage).Values.ToList<MenuItem>(), currentOrder);
        }

        private void AddOtherItemButton_Click(object sender, RoutedEventArgs e)
        {
            Order currentOrder = ((CreateOrderTab)mainWindow.ActiveTabControl.SelectedItem).order;
            ItemListWindow customerListWindow = new ItemListWindow(mainWindow, ItemHelper.getAllItems(DatabaseConnectionHelper.defaultConnection,
                ItemGroup.Other).Values.ToList<MenuItem>(), currentOrder);
        }

        private void RemoveItemButton_Click(object sender, RoutedEventArgs e)
        {
            Order currentOrder = ((CreateOrderTab)mainWindow.ActiveTabControl.SelectedItem).order;
            currentOrder.RemoveItem((MenuItem)OrderListView.SelectedItem);
        }

        private void UpdatePriceButton_Click(object sender, RoutedEventArgs e)
        {
            Order currentOrder = ((CreateOrderTab)mainWindow.ActiveTabControl.SelectedItem).order;
            FormTextBoxPrice.Text = String.Concat("$", Convert.ToString(currentOrder.orderPrice));
        }

        private void OrderItemClicked(object sender, MouseButtonEventArgs e)
        {
            Order menuOrder = ((CreateOrderTab)mainWindow.ActiveTabControl.SelectedItem).order;
            EditItemWindow editWindow = new EditItemWindow(mainWindow, menuOrder.orderItems[OrderListView.SelectedIndex], true);
        }
        #endregion
    }
}
