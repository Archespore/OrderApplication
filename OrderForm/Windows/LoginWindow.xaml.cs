using System;
using System.Windows;
using System.Text.RegularExpressions;
using OrderForm.Helpers;

namespace OrderForm
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private DatabaseConnection connection;

        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public LoginWindow()
        {

        }
        #endregion

        #region Login
        /// <summary>
        /// Login to the actual forms with the specified username
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            String loginName = this.LoginUsername.Text.Trim();
            //First check that a username was even inputted
            if (!String.IsNullOrEmpty(loginName))
            {
                //Test to make sure the username is only letters, otherwise display and error message
                if (Regex.IsMatch(loginName, @"^[\p{L}\s]+$"))
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else { this.LoginError.Text = "Your username contains invalid characters!"; }
            }
            else { this.LoginError.Text = "Your username cannot be blank!"; }
        }
        #endregion
    }
}
