using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using HaLi.AudioInput;
using Speech_To_Text.GoogleSpeech;

namespace Speech_To_Text
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => Speech(3000, "en"));
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
        }

        private string FileDialog()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Wave File|*.wav"
            };

            string output = string.Empty;
            if (dialog.ShowDialog().GetValueOrDefault(false))
            {
                output = dialog.FileName;
            }

            return output;
        }

        private async Task Speech(int length, string language)
        {
            var task = new SpeechTask
            {
                Language = language,
                KeepWavFile = true,
            };

            var data = await task.StartRecord(length);

            Console.WriteLine(data.Text);
        }
    }
}
