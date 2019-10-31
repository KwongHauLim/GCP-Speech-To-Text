using System;
using System.Windows.Input;

namespace Speech_To_Text.View.Command
{
    public class ExitApp : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            App.Current.Shutdown();
        }
    }
}
