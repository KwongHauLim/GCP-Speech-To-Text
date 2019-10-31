using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;

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
                uiJson.Text = dialog.FileName;

                bool valid = false;
                try
                {
                    var json = File.ReadAllText(dialog.FileName);
                    var jo = JObject.Parse(json);
                    valid = jo.ContainsKey("private_key");
                }
                catch
                {
                    valid = false;
                }

                if (valid != isValid)
                    uiValid.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/" + (valid? "tick.png": "cross.png")));
                isValid = valid;
            }
        }
    }
}
