﻿using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using HaLi.AudioInput;
using HaLi.GoogleSpeech;
using SensitiveMode = Speech_To_Text.Setting.GoogleSpeech.SensitiveMode;

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

            var ctrl = Control.Share;
            var setting = ctrl.Setting;
            if (setting.EnableWhenStart && setting.Speech.Sensitive == SensitiveMode.Manual)
            {
                ctrl.ManualOpen();
            }
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
                KeepWavFile = $@"R:\Voice{DateTime.Now.ToString("mmss")}.wav",
            };

            var data = await task.StartRecord(length);

            Console.WriteLine(data.Text);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //clean up notifyicon (would otherwise stay open until application finishes)
            //NotifyIcon.Dispose();
        }
    }
}
