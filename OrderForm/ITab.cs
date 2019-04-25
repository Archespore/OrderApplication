using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrderForm
{
    public interface ITab
    {
        /// <summary>
        /// Display name of the tab
        /// </summary>
        string tabName { get; set; }

        /// <summary>
        /// The command that is run when the tab is closed
        /// </summary>
        ICommand tabClose { get; }

        /// <summary>
        /// The event that is fired when the tab is closed
        /// </summary>
        event EventHandler tabClosedEvent;
    }

    public abstract class Tab : ITab
    {
        public Tab()
        {
            tabClose = new SimpleCommand(x => tabClosedEvent?.Invoke(this, EventArgs.Empty));
        }

        public string tabName { get; set; }
        public ICommand tabClose { get; }
        public event EventHandler tabClosedEvent;
    }
}
