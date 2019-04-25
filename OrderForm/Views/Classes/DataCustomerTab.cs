using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderForm
{
    class DataCustomerTab : Tab
    {
        public Customer customerData { get; set; }
        public string titleName { get; set; }

        public DataCustomerTab(Customer customerData)
        {
            this.customerData = customerData;

            tabName = customerData.customerName;
            titleName = String.Concat("Customer Data: ", tabName);
        }
    }
}
