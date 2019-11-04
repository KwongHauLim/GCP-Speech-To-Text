using System;
using System.Windows.Input;
using RecordMode = Speech_To_Text.Setting.GoogleSpeech.RecordMode;

namespace Speech_To_Text.View.Command
{
    public class ModeStream : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            Control.Share.Setting.Speech.Mode = RecordMode.Stream;
            Control.Share.SaveSetting();
        }
    }
}
