using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using System.ComponentModel;

namespace Speech_To_Text.View.Records
{
    /// <summary>
    /// ViewRecords.xaml 的互動邏輯
    /// </summary>
    public partial class ViewRecords : Window
    {
        private Dictionary<string, FileInfo> dictFiles = new Dictionary<string, FileInfo>();
        private Dictionary<string, string> dictWaves = new Dictionary<string, string>();

        public ViewRecords()
        {
            InitializeComponent();

            if (LicenseManager.UsageMode != LicenseUsageMode.Runtime)
                return;

            var here = new FileInfo(typeof(MainWindow).Assembly.Location);
            DirectoryInfo dir = here.Directory.GetDirectories("Voices")[0];
            FileInfo[] files = dir.GetFiles("Voices*.txt");

            foreach (var item in files.OrderByDescending((item) => item.CreationTime))
            {
                dictFiles[item.Name] = item;
            }

            uiFiles.ItemsSource = dictFiles.Keys;
            uiFiles.SelectedIndex = 0;
        }

        private void uiFiles_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //uiResults.Items.Clear();
            var file = dictFiles[(string)uiFiles.SelectedItem];
            var list = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(file.FullName));
            var small = new Dictionary<string, string>();
            foreach (var pair in list)
            {
                var name = Path.GetFileName(pair.Key);
                dictWaves[name] = pair.Key;
                small[name] = pair.Value;
            }
            uiResults.ItemsSource = small;
        }

        private void uiResults_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //if (e.AddedItems != null && e.AddedItems.Count == 1)
            //{
            //    var item = (KeyValuePair<string, string>)e.AddedItems[0];
            //    if (dictWaves.ContainsKey(item.Key))
            //    {
            //        Process.Start(dictWaves[item.Key]);
            //    }
            //}
        }

        private void BtnClean_Click(object sender, RoutedEventArgs e)
        {
            var ctrl = Control.Share;
            var files = ctrl.directory?.GetFiles() ?? new FileInfo[0];
            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    files[i].Delete();
                }
                catch { }
            }

            this.Close();
        }
    }
}
