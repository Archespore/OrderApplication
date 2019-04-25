using System;

namespace OrderForm
{
    class CreateCustomerTab : Tab
    {
        public string customerName { get; set; }
        public string customerPhone { get; set; }
        public string customerEmail { get; set; }
        public string customerAddress { get; set; }
        public string customerCity { get; set; }
        public string customerState { get; set; }
        public string customerZip { get; set; }

        public CreateCustomerTab()
        {
            tabName = "Create Customer";
        }
    }
}
