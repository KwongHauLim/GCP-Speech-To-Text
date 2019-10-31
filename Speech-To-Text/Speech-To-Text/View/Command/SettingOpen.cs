using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Speech_To_Text.View.Setting;

namespace Speech_To_Text.View.Command
{
    public class SettingOpen : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var popup = new PopupSetting();
            popup.Show();
        }
    }
}
