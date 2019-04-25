using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OrderForm.Windows
{
    /// <summary>
    /// Interaction logic for CustomerListWindow.xaml
    /// </summary>
    public partial class CustomerListWindow : Window
    {
        List<Customer> customerList { get; set; }
        MainWindow parentWindow { get; set; }

        #region Constructor
        public CustomerListWindow(MainWindow parentWindow, List<Customer> customerList)
        {
            InitializeComponent();

            DataContext = this;
            this.customerList = customerList;
            this.parentWindow = parentWindow;

            CustomerList.ItemsSource = customerList;

            parentWindow.openSubWindow(this);
        }
        #endregion

        private void ItemClicked(object sender, MouseButtonEventArgs e)
        {
            parentWindow.activeTabs.Add(new DataCustomerTab((Customer)CustomerList.SelectedItem));
            parentWindow.closeSubWindow();
        }
    }
}
