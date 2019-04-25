using OrderForm.Helpers;
using OrderForm.Windows;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace OrderForm.Views
{
    /// <summary>
    /// Interaction logic for DataCustomerView.xaml
    /// </summary>
    public partial class DataCustomerView : UserControl
    {
        private MainWindow mainWindow;

        #region Constructor
        public DataCustomerView()
        {
            InitializeComponent();

            this.Loaded += ViewLoaded;
        }

        private void ViewLoaded(object sender, RoutedEventArgs e)
        {
            mainWindow = (MainWindow)Window.GetWindow(this);
        }
        #endregion

        #region Click Events
        private void ButtonOrder_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.activeTabs.Add(new CreateOrderTab(FormTextBoxID.Text));
        }
        private void ButtonHistory_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnection connection = mainWindow.databaseConnection;

            //Build a dictionary of values to check for in out SQL query
            Dictionary<string, string> queryValues = new Dictionary<string, string>();
            queryValues.Add("customer_id", FormTextBoxID.Text.Trim());

            string queryString = DatabaseConnectionHelper.BuildSelectStatement("SELECT * FROM Orders WHERE ", queryValues);
            SqlDataReader sqlReader = connection.SQLReaderQuery(queryString);
            List<Order> orderList = DatabaseConnectionHelper.CreateOrderList(sqlReader);
            connection.closeConnection();
            DatabaseConnectionHelper.PopulateOrderList(orderList, connection);
            connection.closeConnection();

            OrderListWindow customerListWindow = new OrderListWindow(mainWindow, orderList);
            FormResult.Text = "Searching for order...   Found (" + orderList.Count + ") result(s)!";
        }
        #endregion
    }
}
