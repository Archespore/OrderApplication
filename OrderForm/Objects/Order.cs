using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderForm
{

    public class Order
    {
        public ObservableCollection<MenuItem> orderItems { get; set; }
        public int orderID { get; set; }
        public string orderCustomer { get; set; }
        public bool orderDelivery { get; set; }
        public bool orderPickup { get; set; }
        public string orderNotes { get; set; }
        public double orderPrice { get; set; }

        #region Constructor
        //This constructor is used when creating an order, we don't need to input these fields since we will get them from the form when we create the order
        public Order() : this(0, "", false, false, "", 0.0) { }
        //This constructor is used when viewing an order
        public Order(int orderID, string orderCustomer, bool orderDelivery, bool orderPickup, string orderNotes, double orderPrice)
        {
            this.orderID = orderID;
            this.orderCustomer = orderCustomer;
            this.orderDelivery = orderDelivery;
            this.orderPickup = orderPickup;
            this.orderNotes = orderNotes;
            this.orderPrice = orderPrice;

            orderItems = new ObservableCollection<MenuItem>();
            orderItems.CollectionChanged += UpdateOrder;
        }
        #endregion

        /// <summary>
        /// Clears ALL current items in the order
        /// </summary>
        public void clearOrder()
        {
            orderItems.Clear();
        }

        /// <summary>
        /// Adds the specified item to the order
        /// </summary>
        public void AddItem(MenuItem item)
        {
            orderItems.Add(item);
        }

        /// <summary>
        /// Removes the specified item to the order
        /// </summary>
        public void RemoveItem(MenuItem item)
        {
            orderItems.Remove(item);
        }

        private void UpdateOrder(object sender, NotifyCollectionChangedEventArgs e)
        {
            orderPrice = 0;
            foreach(MenuItem item in orderItems)
            {
                orderPrice += item.menuItemPrice;
            }
        }
    }
}
