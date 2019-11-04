using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Speech_To_Text.View.Records;

namespace Speech_To_Text.View.Command
{
    public class ViewRecs : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            var popup = new ViewRecords();
            popup.Show();
        }
    }
}
