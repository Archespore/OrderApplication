using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OrderForm.Helpers
{
    public static class FormHelper
    {
        /// <summary>
        /// Recursively finds all children of the specified UIElement
        /// </summary>
        /// <param name="panel">The initial panel</param>
        /// <returns>List of all child elements found under the specified panel</returns>
        public static List<UIElement> FindChildren(Panel panel)
        {
            List<UIElement> foundElements = new List<UIElement>();

            foreach (UIElement element in panel.Children)
            {
                //If the child element is another panel, call the function again
                if(element.GetType().IsSubclassOf(typeof(Panel)))
                {
                    foundElements.AddRange(FindChildren((Panel)element));
                }
                //If the child element is not another panel, add it to the return list
                else
                {
                    foundElements.Add(element);
                }
            }

            return foundElements;
        }
    }
}
