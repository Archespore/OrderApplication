using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OrderForm.Helpers
{
    public enum ItemGroup
    {
        Main = 0,
        Side = 1,
        Other = 2,
        Beverage = 3,
        All = 4
    }

    public enum ItemData
    {
        ID,
        Name,
        Group,
        Price,
        Active,
        Description,
        Modifiers
    }

    public static class ItemHelper
    {
        /// <summary>
        /// Gets all items for the specified group, or for all groups
        /// </summary>
        /// <param name="connection">the connection to use to gather the data</param>
        public static Dictionary<int, MenuItem> getAllItems(DatabaseConnection connection) { return getAllItems(connection, ItemGroup.All); }
        public static Dictionary<int, MenuItem> getAllItems(DatabaseConnection connection, ItemGroup group)
        {
            Dictionary<int, MenuItem> registeredMenuItems = new Dictionary<int, MenuItem>();

            //Build our query string depending on the group we are looking for
            string queryString = "SELECT * FROM MenuItems WHERE menu_item_active=1";
            if (group != ItemGroup.All) { queryString = String.Concat(queryString, " AND menu_item_group=", (int)group); }
            queryString = String.Concat(queryString, ";");

            //Because the MenuItem gathers its own information on creation, we create a list of all itemIDs that are active and belong to the specified group
            SqlDataReader itemReader = connection.SQLReaderQuery(queryString);
            List<int> foundItemIDS = new List<int>();
            while (itemReader.Read()) { foundItemIDS.Add(Convert.ToInt32(itemReader["menu_item_id"])); }
            connection.closeConnection();

            //We then create the item for each itemID we found. We do it this way because I think it's cleaner.
            foreach (int itemID in foundItemIDS) { registeredMenuItems.Add(itemID, new MenuItem(itemID)); }

            return registeredMenuItems;
        }

        /// <summary>
        /// Gets the item data associated with the specified itemID
        /// </summary>
        /// <param name="connection">the connection to use to gather the data</param>
        /// <param name="itemID">The itemID whose data we are getting</param>
        /// <returns>The item data associated with the itemID</returns>
        public static Dictionary<ItemData, object> getItemData(DatabaseConnection connection, int itemID)
        {
            Dictionary<ItemData, object> itemData = new Dictionary<ItemData, object>();

            //Build our query string depending on the item we are looking for
            string queryString = String.Format("SELECT * FROM MenuItems WHERE menu_item_id={0};", itemID);
            SqlDataReader itemReader = connection.SQLReaderQuery(queryString);

            if (itemReader.HasRows)
            {
                itemReader.Read();
                itemData.Add(ItemData.ID, itemReader["menu_item_id"]);
                itemData.Add(ItemData.Name, itemReader["menu_item_name"]);
                itemData.Add(ItemData.Group, itemReader["menu_item_group"]);
                itemData.Add(ItemData.Price, itemReader["menu_item_price"]);
                itemData.Add(ItemData.Active, itemReader["menu_item_active"]);
                itemData.Add(ItemData.Description, itemReader["menu_item_description"]);
                itemData.Add(ItemData.Modifiers, itemReader["menu_item_modifiers"]);
            }

            connection.closeConnection();
            return itemData;
        }

        /// <summary>
        /// Gets the default item modifiers with the specified itemID
        /// </summary>
        /// <param name="connection">the connection to use to gather the data</param>
        /// <param name="itemID">The itemID whose data we are getting</param>
        /// <returns>The item modifiers associated with the itemID</returns>
        public static Dictionary<string, int> getItemModifiers(DatabaseConnection connection, int itemID)
        {
            Dictionary<ItemData, object> itemData = getItemData(connection, itemID);

            //Parse modifiers for this item
            string modifiersString = Convert.ToString(itemData[ItemData.Modifiers]);
            Dictionary<string, int> itemModifiers = parseItemModifiers(modifiersString);

            return itemModifiers;
        }

        /// <summary>
        /// Gets the default item modifiers with the specified itemID
        /// </summary>
        /// <param name="connection">the connection to use to gather the data</param>
        /// <param name="itemID">The itemID whose data we are getting</param>
        /// <returns>The item modifiers associated with the itemID</returns>
        public static Dictionary<string, int> parseItemModifiers(string modifiers)
        {
            Dictionary<string, int> itemModifiers = new Dictionary<string, int>();

            //Parse modifiers for this string
            if (modifiers != String.Empty)
            {
                string[] splitModifiers = modifiers.Split(',');
                foreach (string modifier in splitModifiers)
                {
                    //Modifiers format: "Key:Value"
                    string[] keyValue = modifier.Split(':');
                    string key = keyValue[0];
                    int value = int.Parse(keyValue[1]);
                    itemModifiers.Add(key, value);
                }
            }
            return itemModifiers;
        }
    }
}
