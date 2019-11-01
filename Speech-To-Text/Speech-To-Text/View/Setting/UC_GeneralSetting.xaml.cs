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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Speech_To_Text.View.Setting
{
    /// <summary>
    /// UC_GeneralSetting.xaml 的互動邏輯
    /// </summary>
    public partial class UC_GeneralSetting : UserControl
    {
        private List<string> CodeList = new List<string>();

        public UC_GeneralSetting()
        {
            InitializeComponent();

            CodeList.AddRange(Control.Share.LanguageCodes.Keys);
            uiLanguage.ItemsSource = Control.Share.LanguageCodes.Values;
            uiLanguage.SelectedIndex = 0;
        }

        public void SetLanguageCode(string defaultLanguage)
            => uiLanguage.SelectedIndex = CodeList.IndexOf(defaultLanguage);

        public string GetLanguageCode() 
            => CodeList[uiLanguage.SelectedIndex];
    }
}
