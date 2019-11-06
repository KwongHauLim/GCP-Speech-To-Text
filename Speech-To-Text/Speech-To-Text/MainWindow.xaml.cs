using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using HaLi.AudioInput;
using HaLi.GoogleSpeech;
using RecordMode = Speech_To_Text.Setting.GoogleSpeech.RecordMode;
//using SensitiveMode = Speech_To_Text.Setting.GoogleSpeech.SensitiveMode;
using Hardcodet.Wpf.TaskbarNotification;
using Speech_To_Text.View.Setting;
using System.Windows.Threading;
using System.Collections.Generic;

namespace Speech_To_Text
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Share => (MainWindow)App.Current.MainWindow;
        public TaskbarIcon GetNotify => NotifyIcon;
        public List<string> listBalloon = new List<string>();

        private DispatcherTimer timer;

        public MainWindow()
        {
            try
            {
                Control.WriteLog("App Start");
                InitializeComponent();

                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1.0);
                timer.Tick += VisualUpdate;

                var ctrl = Control.Share;
                var setting = ctrl.Setting;
                bool first = false;
                if (string.IsNullOrWhiteSpace(setting.Speech.Credential))
                {
                    var popup = new PopupSetting();
                    popup.Show();
                    first = true;
                }
                else if (setting.EnableWhenStart)
                {
                    ctrl.ManualOpen();
                    //ctrl.Manual.StartVoice();
                }

                SetMode(setting.Speech.Mode);
                SetLang(ctrl.Language);
                SetActive(!first && setting.EnableWhenStart);

                timer.Start();
            }
            catch (Exception ex)
            {
                Control.WriteLog(ex.Message);
                Control.WriteLog(ex.StackTrace);
                throw ex;
            }
        }

        private void VisualUpdate(object sender, EventArgs e)
        {
            if (listBalloon.Count > 0)
            {
                var msg = listBalloon[0];
                try
                {
                    Control.WriteLog(msg);
                    var notify = Share.GetNotify;
                    notify.ShowBalloonTip("Voice to Text", msg, notify.Icon);
                }
                catch { }
                listBalloon.RemoveAt(0);
            }
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
            //noticModeFile.Visibility = Visibility.Hidden;
            //noticModeStream.Visibility = Visibility.Hidden;

            //if (mode == RecordMode.File)
            //    noticModeFile.Visibility = Visibility.Visible;
            //if (mode == RecordMode.Stream)
            //    noticModeStream.Visibility = Visibility.Visible;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //clean up notifyicon (would otherwise stay open until application finishes)
            NotifyIcon.Dispose();
        }

        /// <summary>
        /// Taskbar icon 彈出氣泡
        /// </summary>
        public static void Balloon(string msg)
            => Share.listBalloon.Add(msg);
    }
}
