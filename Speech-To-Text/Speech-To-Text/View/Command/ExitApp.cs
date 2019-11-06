using System;
using System.IO;
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
            var ctrl = Control.Share;
            if (ctrl.Setting.DeleteWhenExit && ctrl.Setting.DeleteWhenExit)
            {
                var wavs = ctrl.directory?.GetFiles("*.wav", System.IO.SearchOption.TopDirectoryOnly) ?? new FileInfo[0];
                for (int i = 0; i < wavs.Length; i++)
                {
                    try
                    {
                        wavs[i].Delete();
                    }
                    catch { }
                }
            }
            App.Current.Shutdown();
        }
    }
}
