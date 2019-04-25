using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderForm.Helpers
{
    public static class DatabaseConnectionHelper
    {
        public static DatabaseConnection defaultConnection { get; set; }
        /// <summary>
        /// Builds a SQL select statement with the provided strings
        /// </summary>
        /// <param name="initialStatement">the initial select statement to build on</param>
        /// <param name="queryValues">the strings to add together into a select statement</param>
        /// <returns>the full select string</returns>
        public static string BuildSelectStatement(string initialStatement, Dictionary<string, string> queryValues)
        {
            StringBuilder completedSelect = new StringBuilder(initialStatement);
            bool initialValue = true;
            foreach (KeyValuePair<string, string> queryPair in queryValues)
            {
                string queryKey = queryPair.Key;
                string queryValue = queryPair.Value;

                //If either of our query values are empty, we can ignore this entry
                if ((queryKey != String.Empty) && (queryValue != String.Empty))
                {
                    //If this is not the first value in the arguments list, add a comma to the query to specify another value
                    if (!initialValue)
                    {
                        completedSelect.Append(" AND ");
                    }
                    completedSelect.Append(String.Concat(queryKey, "='", queryValue, "'"));
                    initialValue = false;
                }
            }
            //Before we return the string, add the semicolon to signify the end of the statement
            completedSelect.Append(";");
            return completedSelect.ToString();
        }

        /// <summary>
        /// Creates a list of customers from the provided data reader
        /// </summary>
        /// <param name="dataReader">the SqlDataReader to use</param>
        /// <returns>a list of all customers found from the reader</returns>
        public static List<Customer> CreateCustomerList(SqlDataReader dataReader)
        {
            List<Customer> customerList = new List<Customer>();
            while (dataReader.Read())
            {
                //Try to parse the provided data as a customer
                try
                {
                    //Gather customer data
                    string customerID = Convert.ToString(dataReader["customer_id"]);
                    string customerName = Convert.ToString(dataReader["customer_name"]);
                    string customerPhone = Convert.ToString(dataReader["customer_phone"]);
                    string customerEmail = Convert.ToString(dataReader["customer_email"]);
                    string customerAddress = Convert.ToString(dataReader["customer_address"]);
                    string customerCity = Convert.ToString(dataReader["customer_city"]);
                    string customerState = Convert.ToString(dataReader["customer_state"]);
                    string customerZip = Convert.ToString(dataReader["customer_zip"]);

                    Customer foundCustomer = new Customer(customerID, customerName, customerPhone, customerEmail, customerAddress, customerCity, customerState, customerZip);
                    customerList.Add(foundCustomer);
                }
                //If the data supplied is NOT a customer, we can't do anything with it.
                catch (Exception error)
                {
                    Console.WriteLine("There was an error when trying to parse SQL data as a customer object!");
                    Console.WriteLine(error.ToString());
                }
            }
            return customerList;
        }

        /// <summary>
        /// Creates a list of orders from the provided data reader
        /// </summary>
        /// <param name="dataReader">the SqlDataReader to use</param>
        /// <returns>a list of all orders found from the reader</returns>
        public static List<Order> CreateOrderList(SqlDataReader dataReader)
        {
            List<Order> orderList = new List<Order>();
            while (dataReader.Read())
            {
                //Try to parse the provided data as an order
                try
                {
                    //Gather order data
                    int orderID = Convert.ToInt32(dataReader["order_id"]);
                    string orderCustomer = Convert.ToString(dataReader["customer_id"]);
                    bool orderFulfillment = Convert.ToBoolean(dataReader["order_fulfillment"]);
                    string orderNotes = Convert.ToString(dataReader["order_notes"]);
                    double orderPrice = Convert.ToDouble(dataReader["order_total"]);

                    Order foundOrder = new Order(orderID, orderCustomer, orderFulfillment, !orderFulfillment, orderNotes, orderPrice);
                    orderList.Add(foundOrder);
                }
                //If the data supplied is NOT an order, we can't do anything with it.
                catch (Exception error)
                {
                    Console.WriteLine("There was an error when trying to parse SQL data as a customer object!");
                    Console.WriteLine(error.ToString());
                }
            }
            return orderList;
        }

        /// <summary>
        /// Populates a list of orders with their order items from the order lines database
        /// </summary>
        /// <param name="orderList">the order list to populate</param>
        /// /// <param name="connection">the DatabaseConnection to use</param>
        /// <returns>a populated list of orders</returns>
        public static void PopulateOrderList(List<Order> orderList, DatabaseConnection connection)
        {
            foreach (Order order in orderList)
            {
                //Build query string
                string queryString = String.Format("SELECT menu_item_id, order_item_modifiers FROM OrderLines WHERE order_id={0}", order.orderID);

                SqlDataReader dataReader = connection.SQLReaderQuery(queryString);
                while (dataReader.Read())
                {
                    //Try to parse the provided data as an item
                    try
                    {
                        //Gather item data
                        int itemID = Convert.ToInt32(dataReader["menu_item_id"]);
                        string itemModifiers = Convert.ToString(dataReader["order_item_modifiers"]);

                        MenuItem item = new MenuItem(itemID, itemModifiers);
                        order.orderItems.Add(item);
                    }
                    //If the data supplied is NOT an item, we can't do anything with it.
                    catch (Exception error)
                    {
                        Console.WriteLine("There was an error when trying to parse SQL data as a customer object!");
                        Console.WriteLine(error.ToString());
                    }
                }
                connection.closeConnection();
            }
        }
    }
}
