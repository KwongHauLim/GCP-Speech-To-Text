using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Speech_To_Text.View.Command
{
    public class VoiceEnable : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            Control.Share.ManualOpen();
        }
    }
}
