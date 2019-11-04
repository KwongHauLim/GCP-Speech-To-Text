using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Speech_To_Text.View.Command
{
    public class GoogleLanguage : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            var lang =  (parameter as string) ?? "en";
            Control.Share.Language = lang;
            var main = (MainWindow)App.Current.MainWindow;
            main.SetLang(lang);
        }
    }
}
