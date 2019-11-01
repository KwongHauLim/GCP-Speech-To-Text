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

            var ctrl = Control.Share;
            var setting = ctrl.Setting;
            ucGeneral.uiStartEnable.IsChecked = setting.EnableWhenStart;
            ucGeneral.SetLanguageCode(setting.DefaultLanguage);
            ucGeneral.uiFilterBelow.Text = setting.MinLength.ToString("0");
            ucGeneral.uiMaxLength.Text = setting.MaxLength.ToString("0");
            ucGeneral.uiKeepWav.IsChecked = setting.KeepWavFile;
            ucGeneral.uiDelLeave.IsChecked = setting.DeleteWhenExit;
            ucGoogle.uiJson.Text = setting.Speech.Credential;
            ucGoogle.Mode = setting.Speech.Mode;
            ucGoogle.Senitive = setting.Speech.Sensitive;
            ucGoogle.ValidateJson(ucGoogle.uiJson.Text);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var ctrl = Control.Share;
            var setting = ctrl.Setting;
            setting.EnableWhenStart = ucGeneral.uiStartEnable.IsChecked.GetValueOrDefault(true);
            setting.DefaultLanguage = ucGeneral.GetLanguageCode();
            setting.MinLength = double.Parse(ucGeneral.uiFilterBelow.Text);
            setting.MaxLength = double.Parse(ucGeneral.uiMaxLength.Text);
            setting.KeepWavFile = ucGeneral.uiKeepWav.IsChecked.GetValueOrDefault(true);
            setting.DeleteWhenExit = ucGeneral.uiDelLeave.IsChecked.GetValueOrDefault(true);
            setting.Speech.Credential = ucGoogle.uiJson.Text;
            setting.Speech.Mode = ucGoogle.Mode;
            setting.Speech.Sensitive = ucGoogle.Senitive;
            ctrl.SaveSetting();
        }
    }
}
