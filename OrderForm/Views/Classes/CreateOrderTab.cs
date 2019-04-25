using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderForm
{
    class CreateOrderTab : Tab
    {
        public Order order { get; set; }
        public string orderCustomer { get; set; }
        public bool orderDelivery { get; set; }
        public bool orderPickup { get; set; }
        public string orderNotes { get; set; }

        #region Constructor
        public CreateOrderTab() : this(String.Empty) { }
        public CreateOrderTab(string id)
        {
            tabName = "Create Order";

            order = new Order();
            orderCustomer = id;
            orderDelivery = false;
            orderPickup = false;
            orderNotes = String.Empty;
        }
        #endregion
    }
}
