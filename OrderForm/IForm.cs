using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace OrderForm
{
    public interface IForm
    {
        //List of all UIElements in this form
        List<UIElement> formElements { get; set; }

        /// <summary>
        /// Completely resets the form, this means any InputElements and ErrorBlocks
        /// </summary>
        void ResetForm();

        /// <summary>
        /// Resets all TextBlocks in the form that are tagged as ErrorBlock. 
        /// </summary>
        void ResetFormErrors();

        /// <summary>
        /// Checks the forms for errors, this logic is entirely dependent on the form being checked
        /// </summary>
        /// <returns></returns>
        bool CheckForErrors();

        /// <summary>
        /// Returns all form elements for the form, usually called once on tab constructor
        /// </summary>
        /// <param name="panel">The panel that contains the form for this tab</param>
        /// <returns>a list of all form elements that are not panels</returns>
        List<UIElement> FindFormElements(Panel panel);
    }
}
