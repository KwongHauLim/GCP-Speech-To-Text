using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using HaLi.AudioInput;
using HaLi.GoogleSpeech;
using RecordMode = Speech_To_Text.Setting.GoogleSpeech.RecordMode;
using SensitiveMode = Speech_To_Text.Setting.GoogleSpeech.SensitiveMode;
using Hardcodet.Wpf.TaskbarNotification;

namespace Speech_To_Text
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TaskbarIcon GetNotify => NotifyIcon;

        public MainWindow()
        {
            InitializeComponent();

            var ctrl = Control.Share;
            var setting = ctrl.Setting;
            if (setting.EnableWhenStart)
            {
                ctrl.ManualOpen();
            }

            SetMode(setting.Speech.Mode);
            SetLang(ctrl.Language);
            SetActive(setting.EnableWhenStart);
        }

        internal void SetActive(bool enable)
        {
            if (enable)
            {
                noticEnable.Visibility = Visibility.Visible;
                noticDisable.Visibility = Visibility.Hidden;
            }
            else
            {
                noticEnable.Visibility = Visibility.Hidden;
                noticDisable.Visibility = Visibility.Visible;
            }
        }

        internal void SetLang(string lang)
        {
            notiLangEng.Visibility = lang.Equals("en") ? Visibility.Visible : Visibility.Hidden;
            notiLangZh.Visibility = lang.Equals("zh") ? Visibility.Visible : Visibility.Hidden;
            notiLangYue.Visibility = lang.Equals("yue-Hant-HK") ? Visibility.Visible : Visibility.Hidden;
        }

        internal void SetMode(RecordMode mode)
        {
            noticModeFile.Visibility = Visibility.Hidden;
            noticModeStream.Visibility = Visibility.Hidden;

            if (mode == RecordMode.File)
                noticModeFile.Visibility = Visibility.Visible;
            if (mode == RecordMode.Stream)
                noticModeStream.Visibility = Visibility.Visible;
        }

        //private void btnTest_Click(object sender, RoutedEventArgs e)
        //{
        //    Task.Run(() => Speech(3000, "en"));
        //}

        //private void btnOpen_Click(object sender, RoutedEventArgs e)
        //{
        //}

        //private string FileDialog()
        //{
        //    var dialog = new OpenFileDialog
        //    {
        //        Filter = "Wave File|*.wav"
        //    };

        //    string output = string.Empty;
        //    if (dialog.ShowDialog().GetValueOrDefault(false))
        //    {
        //        output = dialog.FileName;
        //    }

        //    return output;
        //}

        //private async Task Speech(int length, string language)
        //{
        //    var task = new SpeechTask
        //    {
        //        Language = language,
        //        KeepWavFile = $@"R:\Voice{DateTime.Now.ToString("mmss")}.wav",
        //    };

        //    var data = await task.StartRecord(length);

        //    Console.WriteLine(data.Text);
        //}

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //clean up notifyicon (would otherwise stay open until application finishes)
            NotifyIcon.Dispose();
        }
    }
}
