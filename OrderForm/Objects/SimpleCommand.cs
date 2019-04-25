using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrderForm
{
    class SimpleCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        Action<object> commandAction;

        public SimpleCommand(Action<object> commandAction)
        {
            this.commandAction = commandAction;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute()
        {
            Execute(null);
        }

        public void Execute(object parameter)
        {
            commandAction(parameter);
        }
    }
}
