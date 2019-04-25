using OrderForm.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OrderForm.Views
{
    /// <summary>
    /// Interaction logic for DataOrderView.xaml
    /// </summary>
    public partial class DataOrderView : UserControl
    {
        private MainWindow mainWindow;

        #region Constructor
        public DataOrderView()
        {
            InitializeComponent();

            this.Loaded += ViewLoaded;
        }

        private void ViewLoaded(object sender, RoutedEventArgs e)
        {
            mainWindow = (MainWindow)Window.GetWindow(this);
        }
        #endregion

        #region Click Events
        private void OrderItemClicked(object sender, MouseButtonEventArgs e)
        {
            Order menuOrder = ((DataOrderTab)mainWindow.ActiveTabControl.SelectedItem).order;
            EditItemWindow editWindow = new EditItemWindow(mainWindow, menuOrder.orderItems[OrderListView.SelectedIndex], false);
        }
        #endregion
    }
}
