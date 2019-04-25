using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for EditItemWindow.xaml
    /// </summary>
    public partial class EditItemWindow : Window
    {
        public MainWindow parentWindow { get; set; }
        public MenuItem menuItem { get; set; }
        public bool editable { get; set; }

        public List<KeyValuePair<string, int>> modifiersPairList { get; set; }

        #region Constructor
        public EditItemWindow(MainWindow parentWindow, MenuItem menuItem) : this(parentWindow, menuItem, true) { }
        public EditItemWindow(MainWindow parentWindow, MenuItem menuItem, bool editable)
        {
            InitializeComponent();

            this.parentWindow = parentWindow;
            this.menuItem = menuItem;
            this.editable = editable;

            modifiersPairList = menuItem.menuItemModifiers.ToList();
            DataContext = modifiersPairList;

            parentWindow.openSubWindow(this);

            //Check for the Dressing or SoftDrink modifier and display the panel if they exist
            foreach (KeyValuePair<string, int> keyValuePair in modifiersPairList)
            {
                if (keyValuePair.Key.Equals("Dressing")) { loadDressingPanel(keyValuePair.Value); }
                if (keyValuePair.Key.Equals("SoftDrink")) { loadSodaPanel(keyValuePair.Value); }
            }

            //Remove the Dressing and SoftDrink modifiers since those are determined through a different method
            modifiersPairList.RemoveAll(modifier => modifier.Key == "Dressing");
            modifiersPairList.RemoveAll(modifier => modifier.Key == "SoftDrink");

            //Finally, set the list view to show our modifiers
            ModifierList.ItemsSource = modifiersPairList;
        }
        #endregion

        #region Click Events
        private void AddModifierButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = ModifierList.SelectedIndex;
            if (selectedIndex != -1)
            {
                KeyValuePair<string, int> modifierPair = modifiersPairList[selectedIndex];
                menuItem.menuItemModifiers[modifierPair.Key]++;
                modifiersPairList = menuItem.menuItemModifiers.ToList();
                modifiersPairList.RemoveAll(modifier => modifier.Key == "Dressing");
                modifiersPairList.RemoveAll(modifier => modifier.Key == "SoftDrink");
                ModifierList.ItemsSource = modifiersPairList;
            }
        }

        private void RemoveModifierButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = ModifierList.SelectedIndex;
            if (selectedIndex != -1)
            {
                KeyValuePair<string, int> modifierPair = modifiersPairList[selectedIndex];
                menuItem.menuItemModifiers[modifierPair.Key]--;
                modifiersPairList = menuItem.menuItemModifiers.ToList();
                modifiersPairList.RemoveAll(modifier => modifier.Key == "Dressing");
                modifiersPairList.RemoveAll(modifier => modifier.Key == "SoftDrink");
                ModifierList.ItemsSource = modifiersPairList;
            }
        }

        private void DressingRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            int selectedValue = Convert.ToInt32(((RadioButton)sender).Tag);
            menuItem.menuItemModifiers["Dressing"] = selectedValue;
        }

        private void SodaRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            int selectedValue = Convert.ToInt32(((RadioButton)sender).Tag);
            menuItem.menuItemModifiers["SoftDrink"] = selectedValue;
        }
        #endregion

        #region Load Panel Methods
        /// <summary>
        /// Loads the Dressing panel if the item we are modifying contains a modifier for dressing
        /// </summary>
        private void loadDressingPanel(int value)
        {
            DressingPanel.Visibility = Visibility.Visible;
            switch(value)
            {
                case 0:
                    RanchRadioButton.IsChecked = true;
                    break;
                case 1:
                    ItalianRadioButton.IsChecked = true;
                    break;
                case 2:
                    CaesarRadioButton.IsChecked = true;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Loads the Soda panel if the item we are modifying contains a modifier for soft drinks
        /// </summary>
        private void loadSodaPanel(int value)
        {
            SodaPanel.Visibility = Visibility.Visible;
            switch (value)
            {
                case 0:
                    CokeRadioButton.IsChecked = true;
                    break;
                case 1:
                    DietCokeRadioButton.IsChecked = true;
                    break;
                case 2:
                    SpriteRadioButton.IsChecked = true;
                    break;
                case 3:
                    MntDewRadioButton.IsChecked = true;
                    break;
                case 4:
                    FantaRadioButton.IsChecked = true;
                    break;
                case 5:
                    RootbeerRadioButton.IsChecked = true;
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
