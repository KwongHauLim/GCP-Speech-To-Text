﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using HaLi.AudioInput;
using RecordMode = Speech_To_Text.Setting.GoogleSpeech.RecordMode;

namespace Speech_To_Text.View.Manual
{
    /// <summary>
    /// ManualUI.xaml 的互動邏輯
    /// </summary>
    public partial class ManualUI : Window
    {
        public bool IsPressed { get; set; }

        private VoiceClip voice;

        /// <summary>
        /// For scale viusal of volume
        /// </summary>
        public double VisualScale
        {
            get { return visScale.ScaleX; }
            set { visScale.ScaleX = visScale.ScaleY = value; }
        }

        private DispatcherTimer timer;

        public ManualUI()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.03);
            timer.Tick += VisualUpdate;
        }

        private void VisualUpdate(object sender, EventArgs e)
        {
            if (IsPressed)
            {
                VisualScale = Lerp(1.0, 1.5, Microphone.Volume);
            }
            else if (VisualScale.CompareTo(1.0) != 0)
                VisualScale = 1.0;

            double Lerp(double a, double b, float t) => a * (1 - t) + b * t;
        }

        private void uiMove_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void uiMic_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                IsPressed = true;
                timer.Start();

                voice = new VoiceClip();
                voice.StartVoice(RecordMode.File);
            }
        }

        private void uiMic_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released && IsPressed)
            {
                IsPressed = false;
                var result = voice.StopVoice();
                timer.Stop();

                if (result != null && !string.IsNullOrWhiteSpace(result.Text))
                {
                    Control.InputText(result.Text);
                }
            }
        }
    }
}