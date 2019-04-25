using OrderForm.Helpers;
using OrderForm.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OrderForm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string WINDOW_TITLE = "Order Form";

        private string username;
        public string windowName { get; set; }
        public Window subWindow { get; set; }
        public DatabaseConnection databaseConnection { get; }
        public ObservableCollection<ITab> activeTabs { get; }

        #region Constructor
        /// <summary>
        /// Main Constructor
        /// </summary>
        /// <param name="username">Username to use for this application</param>
        public MainWindow()
        {
            InitializeComponent();

            //Initialize database connection and test the connection
            databaseConnection = new DatabaseConnection();
            if (!databaseConnection.openConnection())
            {
                MessageBox.Show("There was an error connecting to the SQL server, the application will not close.", "Startup Error!");
                this.Close();
            }
            databaseConnection.closeConnection();

            //Set the default database connection in the database connection helper.
            DatabaseConnectionHelper.defaultConnection = databaseConnection;

            DataContext = this;
            username = WindowsIdentity.GetCurrent().Name;
            windowName = String.Concat(WINDOW_TITLE, " - ", username);
            subWindow = null;

            activeTabs = new ObservableCollection<ITab>();
            activeTabs.CollectionChanged += ActiveTabs_Changed;
            Closed += WindowClosed;
        }
        #endregion

        #region Sub Window Functions
        public void openSubWindow(Window window)
        {
            if (subWindow != null) { subWindow.Close(); }
            this.subWindow = window;
            window.Show();
        }

        public void closeSubWindow()
        {
            if (subWindow != null)
            {
                subWindow.Close();
                subWindow = null;
            }
        }
        private void WindowClosed(object sender, EventArgs e)
        {
            closeSubWindow();
        }
        #endregion

        private void ActiveTabs_Changed(object sender, NotifyCollectionChangedEventArgs calledEvent)
        {
            //Find out the action that caused this event to trigger
            switch (calledEvent.Action)
            {
                //If an item was added, add the tab to the tab collection and add a listener
                case NotifyCollectionChangedAction.Add:
                    //Get the newly added item that called this event
                    ITab addedItem = (ITab)calledEvent.NewItems[0];
                    addedItem.tabClosedEvent += ActiveTabs_Closed;
                    break;
                //If an item was removed, remove the tab from the tab collection and remove the listener
                case NotifyCollectionChangedAction.Remove:
                    //Get the newly removed item that called this event
                    ITab removedItem = (ITab)calledEvent.OldItems[0];
                    removedItem.tabClosedEvent -= ActiveTabs_Closed;
                    break;
                //In case it was neither of these actions, we can ignore this event.
                default:
                    break;
            }
        }

        private void ActiveTabs_Closed(object sender, EventArgs e)
        {
            ITab closedTab = (ITab)sender;
            activeTabs.Remove(closedTab);
        }

        #region Menu Button Click Events
        private void CreateCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            activeTabs.Add(new CreateCustomerTab());
        }

        private void CreateOrderButton_Click(object sender, RoutedEventArgs e)
        {
            activeTabs.Add(new CreateOrderTab());
        }

        private void SearchCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            activeTabs.Add(new SearchCustomerTab());
        }
        private void SearchOrderButton_Click(object sender, RoutedEventArgs e)
        {
            activeTabs.Add(new SearchOrderTab());
        }
        private void ViewMenuButton_Click(object sender, RoutedEventArgs e)
        {
            activeTabs.Add(new ViewMenuTab());
        }
        #endregion
    }
}
