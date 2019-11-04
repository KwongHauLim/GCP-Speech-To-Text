using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Input;

namespace Speech_To_Text.View.Command
{
    public class VoiceFolder : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            var here = new FileInfo(typeof(MainWindow).Assembly.Location);
            DirectoryInfo dir = here.Directory.GetDirectories("Voices")[0];
            //System.Diagnostics.Process.Start(dir.FullName); // Bug in .net core 3.0

            var psi = new ProcessStartInfo
            {
                FileName = "cmd",
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = $"/c start {dir.FullName}"
            };
            Process.Start(psi);
        }
    }
}
