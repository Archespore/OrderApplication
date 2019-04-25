using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderForm
{
    class DataOrderTab : Tab
    {
        public Order order { get; set; }
        public DataOrderTab(Order order)
        {
            this.order = order;

            tabName = String.Concat("Order #", order.orderID);
        }
    }
}
