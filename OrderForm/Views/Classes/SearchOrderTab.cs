using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderForm
{
    class SearchOrderTab : Tab
    {
        public string customerID { get; set; }
        public string orderID { get; set; }
        public SearchOrderTab()
        {
            tabName = "Search for Order";
        }
    }
}
