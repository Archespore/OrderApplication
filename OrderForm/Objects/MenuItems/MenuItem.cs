using OrderForm.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderForm
{
    public class MenuItem
    {
        public int menuItemID { get; set; }
        public string menuItemName { get; set; }
        public ItemGroup menuItemGroup { get; set; }
        public double menuItemPrice { get; set; }
        public string menuItemDescription { get; set; }
        public Dictionary<string, int> menuItemModifiers { get; set; }

        #region Constructor
        public MenuItem(int itemID) : this(itemID, String.Empty) { }
        public MenuItem(int itemID, string modifiers)
        {
            DatabaseConnection connection = new DatabaseConnection();
            Dictionary<ItemData, object> itemData = ItemHelper.getItemData(connection, itemID);

            //Check to make sure the item was found
            if (itemData.Count != 0)
            {
                menuItemName = Convert.ToString(itemData[ItemData.Name]);
                menuItemGroup = ((ItemGroup)Convert.ToInt32(itemData[ItemData.Group]));
                menuItemPrice = Convert.ToDouble(itemData[ItemData.Price]);
                menuItemDescription = Convert.ToString(itemData[ItemData.Description]);
                if (modifiers.Equals(String.Empty)) { menuItemModifiers = ItemHelper.getItemModifiers(connection, itemID); }
                else { menuItemModifiers = ItemHelper.parseItemModifiers(modifiers); }
            }
            menuItemID = itemID;

            connection.closeConnection();
        }
        #endregion

        public string ConvertModifiersToString()
        {
            StringBuilder finishedString = new StringBuilder();
            bool initialAppend = true;
            foreach(KeyValuePair<string, int> keyValuePair in menuItemModifiers)
            {
                if (!initialAppend) { finishedString.Append(","); }
                finishedString.Append(String.Concat(keyValuePair.Key, ":", keyValuePair.Value));
                initialAppend = false;
            }
            return finishedString.ToString();
        }
    }
}
