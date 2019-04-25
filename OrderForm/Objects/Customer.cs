using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderForm
{
    public class Customer
    {
        public string customerID { set; get; }
        public string customerName { set; get; }
        public string customerPhone { set; get; }
        public string customerEmail { set; get; }
        public string customerAddress { set; get; }
        public string customerCity { set; get; }
        public string customerState { set; get; }
        public string customerZip { set; get; }
        public Customer(string id, string name, string phone, string email, string address, string city, string state, string zip)
        {
            customerID = id;
            customerName = name;
            customerPhone = phone;
            customerEmail = email;
            customerAddress = address;
            customerCity = city;
            customerState = state;
            customerZip = zip;
        }
    }
}
