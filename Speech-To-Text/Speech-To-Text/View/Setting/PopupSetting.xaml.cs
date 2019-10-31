using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Speech_To_Text.View.Setting
{
    /// <summary>
    /// PopupSetting.xaml 的互動邏輯
    /// </summary>
    public partial class PopupSetting : Window
    {
        public PopupSetting()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var ctrl = Control.Share;
            var setting = ctrl.Setting;
            setting.Speech.Credential = ucGoogle.uiJson.Text;
            setting.Speech.Mode = ucGoogle.Mode;
            setting.Speech.Sensitive = ucGoogle.Senitive;
            ctrl.SaveSetting();
        }
    }
}
