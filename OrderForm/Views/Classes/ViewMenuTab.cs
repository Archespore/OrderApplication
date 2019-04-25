using OrderForm.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderForm
{
    class ViewMenuTab : Tab
    {
        public List<MenuItem> mainItems { get; set; }
        public List<MenuItem> sideItems { get; set; }
        public List<MenuItem> otherItems { get; set; }
        public List<MenuItem> drinkItems { get; set; }

        #region Constructor
        public ViewMenuTab()
        {
            tabName = "View Menu";

            DatabaseConnection connection = DatabaseConnectionHelper.defaultConnection;

            mainItems = ItemHelper.getAllItems(connection, ItemGroup.Main).Values.ToList<MenuItem>();
            sideItems = ItemHelper.getAllItems(connection, ItemGroup.Side).Values.ToList<MenuItem>();
            otherItems = ItemHelper.getAllItems(connection, ItemGroup.Other).Values.ToList<MenuItem>();
            drinkItems = ItemHelper.getAllItems(connection, ItemGroup.Beverage).Values.ToList<MenuItem>();
        }
        #endregion
    }
}
