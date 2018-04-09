using System;
using System.Windows.Input;

namespace PositionSystem.UI.Infrastructure
{
    public class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;
        Action<object> executeMethod;
        Func<object, bool> canExecuteMethod;

        public Command(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
        {
            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }

        public bool CanExecute(object parameter)
        {
            return canExecuteMethod(parameter);
        }

        public void Execute(object parameter)
        {
            executeMethod(parameter);
        }
    }
}
