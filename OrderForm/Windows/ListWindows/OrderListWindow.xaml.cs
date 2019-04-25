using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OrderForm.Windows
{
    /// <summary>
    /// Interaction logic for OrderListWindow.xaml
    /// </summary>
    public partial class OrderListWindow : Window
    {
        List<Order> orderList { get; set; }
        MainWindow parentWindow { get; set; }

        #region Constructor
        public OrderListWindow(MainWindow parentWindow, List<Order> orderList)
        {
            InitializeComponent();

            DataContext = this;
            this.orderList = orderList;
            this.parentWindow = parentWindow;

            OrderList.ItemsSource = orderList;

            parentWindow.openSubWindow(this);
        }
        #endregion

        private void ItemClicked(object sender, MouseButtonEventArgs e)
        {
            parentWindow.activeTabs.Add(new DataOrderTab((Order)OrderList.SelectedItem));
            parentWindow.closeSubWindow();
        }
    }
}
