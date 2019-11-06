using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            try
            {
                InitializeComponent();

                if (LicenseManager.UsageMode != LicenseUsageMode.Runtime)
                    return;

                var ctrl = Control.Share;
                var setting = ctrl.Setting;
                ucGeneral.uiStartEnable.IsChecked = setting.EnableWhenStart;
                ucGeneral.SetLanguageCode(setting.DefaultLanguage);
                //ucGeneral.uiFilterBelow.Value = setting.MinLength;
                //ucGeneral.uiMaxLength.Value = setting.MaxLength;
                ucGeneral.uiClipLength.Text = setting.ClipLength.ToString("0");
                ucGeneral.uiKeepWav.IsChecked = setting.KeepWavFile;
                ucGeneral.uiDelLeave.IsChecked = setting.DeleteWhenExit;
                ucGoogle.uiJson.Text = setting.Speech.Credential;
                ucGoogle.Mode = setting.Speech.Mode;
                //ucGoogle.Senitive = setting.Speech.Sensitive;
                ucGoogle.ValidateJson(ucGoogle.uiJson.Text);

                if (string.IsNullOrWhiteSpace(setting.Speech.Credential))
                {
                    uiTabs.SelectedIndex = 1;
                }
            }
            catch (Exception ex)
            {
                Control.WriteLog(ex.Message);
                Control.WriteLog(ex.StackTrace);
                throw ex;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var ctrl = Control.Share;
            var setting = ctrl.Setting;
            setting.EnableWhenStart = ucGeneral.uiStartEnable.IsChecked.GetValueOrDefault(true);
            setting.DefaultLanguage = ucGeneral.GetLanguageCode();
            //setting.MinLength = ucGeneral.uiFilterBelow.Value.GetValueOrDefault(0.0);
            //setting.MaxLength = ucGeneral.uiMaxLength.Value.GetValueOrDefault(60.0);
            setting.ClipLength = double.Parse(ucGeneral.uiClipLength.Text);
            setting.KeepWavFile = ucGeneral.uiKeepWav.IsChecked.GetValueOrDefault(false);
            setting.DeleteWhenExit = ucGeneral.uiDelLeave.IsChecked.GetValueOrDefault(false);
            setting.Speech.Credential = ucGoogle.uiJson.Text;
            setting.Speech.Mode = ucGoogle.Mode;
            //setting.Speech.Sensitive = ucGoogle.Senitive;
            ctrl.SaveSetting();
            ctrl.Language = setting.DefaultLanguage;
        }
    }
}
