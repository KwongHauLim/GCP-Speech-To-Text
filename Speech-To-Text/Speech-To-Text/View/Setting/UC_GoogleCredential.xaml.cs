using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using RecordMode = Speech_To_Text.Setting.GoogleSpeech.RecordMode;
//using SensitiveMode = Speech_To_Text.Setting.GoogleSpeech.SensitiveMode;

namespace Speech_To_Text.View.Setting
{
    /// <summary>
    /// UC_GoogleCredential.xaml 的互動邏輯
    /// </summary>
    public partial class UC_GoogleCredential : UserControl
    {
        private bool isValid = false;

        public UC_GoogleCredential()
        {
            InitializeComponent();
        }

        public RecordMode Mode
        {
            get
            {
                if (uiModeFile.IsChecked.GetValueOrDefault(true))
                    return RecordMode.File;
                //else if (uiModeStream.IsChecked.GetValueOrDefault(true))
                //    return RecordMode.Stream;
                else
                    return RecordMode.None;
            }
            set
            {
                uiModeFile.IsChecked = value == RecordMode.File;
                //uiModeStream.IsChecked = value == RecordMode.Stream;
            }
        }

        //private SensitiveMode sensitive = SensitiveMode.High;
        //public SensitiveMode Senitive
        //{
        //    get => sensitive;
        //    set
        //    {
        //        sensitive = value;
        //    }
        //}

        private void btnJson_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                FileName = "GoogleCredential.json",
                Filter = "Json File|*.json;*.txt|Any File|*.*",
                Multiselect = false
            };

            if (dialog.ShowDialog().GetValueOrDefault(false))
            {
                if (ValidateJson(dialog.FileName))
                {
                    var here = new FileInfo(typeof(MainWindow).Assembly.Location);
                    var folder = here.Directory;
                    var path = Path.Combine(folder.FullName, "GoogleCredential.json");
                    if (File.Exists(path))
                        File.Delete(path);
                    File.Copy(dialog.FileName, path); // 複製入Program 相同folder
                    uiJson.Text = Path.GetRelativePath(folder.FullName, path);
                }
            }
        }

        /// <summary>
        /// 簡單檢查
        /// </summary>
        public bool ValidateJson(string path)
        {
            bool valid = false;
            try
            {
                var json = File.ReadAllText(path);
                var jo = JObject.Parse(json);
                valid = jo.ContainsKey("private_key");
            }
            catch
            {
                valid = false;
            }

            uiJson.Text = path;

            if (valid != isValid)
                uiValid.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/" + (valid ? "tick.png" : "cross.png")));
            return isValid = valid;
        }

        //private void btnSenHigh_Click(object sender, RoutedEventArgs e)
        //{
        //    Senitive = SensitiveMode.High;
        //}

        //private void btnSenLow_Click(object sender, RoutedEventArgs e)
        //{
        //    Senitive = SensitiveMode.Low;
        //}

        //private void btnSenMan_Click(object sender, RoutedEventArgs e)
        //{
        //    Senitive = SensitiveMode.Manual;
        //}
    }
}
