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
    /// Interaction logic for ItemListWindow.xaml
    /// </summary>
    public partial class ItemListWindow : Window
    {
        Order order { get; set; }
        List<MenuItem> menuList { get; set; }
        MainWindow parentWindow { get; set; }

        #region Constructor
        public ItemListWindow(MainWindow parentWindow, List<MenuItem> menuList, Order order)
        {
            InitializeComponent();

            DataContext = this;
            this.menuList = menuList;
            this.parentWindow = parentWindow;
            this.order = order;

            ItemList.ItemsSource = menuList;

            parentWindow.openSubWindow(this);
        }
        #endregion

        private void ItemClicked(object sender, MouseButtonEventArgs e)
        {
            order.AddItem(menuList[ItemList.SelectedIndex]);
            parentWindow.closeSubWindow();
        }
    }
}
