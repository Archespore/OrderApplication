using System;
using System.Windows;

namespace OrderForm.Helpers
{
    public static class FrameworkElementHelper
    {
        /// <summary>
        /// Checks to make sure the specified element's tag matches the provided string
        /// </summary>
        /// <param name="element">The element's tag we are checking</param>
        /// <param name="tagString">the string we want to check for a match</param>
        /// <returns>True/False</returns>
        public static bool checkElementTag(FrameworkElement element, string tagString)
        {
            //Check to make sure the element's tag is even a string
            object elementTag = element.Tag;
            if (elementTag != null && elementTag.GetType() == typeof(string))
            {
                string elementTagString = (string)elementTag;
                //If the element's tag matches the provided string, return true
                if (String.Compare(tagString, elementTagString) == 0) { return true; }
            }
            //Otherwise, return false
            return false;
        }
    }
}
