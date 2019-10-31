using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HaLi.Tools.Encryption;
using Newtonsoft.Json;
using Speech_To_Text.View.Manual;

namespace Speech_To_Text
{
    public class Control
    {
        private static Control _ptr;
        public static Control Share => _ptr = _ptr ?? new Control();

        private readonly string path = "Setting.json";
        public Setting Setting { get; set; }

        public ManualUI Manual { get; private set; }

        private Control()
        {
            if (File.Exists(path))
            {
                try
                {
                    var json = File.ReadAllText(path);
                    json = Crypto.Decrypt(json);
                    Setting = JsonConvert.DeserializeObject<Setting>(json);
                }
                catch 
                {
                    Setting = null;
                }
            }

            if (Setting == null)
                Setting = new Setting();
        }

        internal void SaveSetting()
        {
            try
            {
                var json = JsonConvert.SerializeObject(Setting);
                json = Crypto.Encrypt(json);
                File.WriteAllText(path, json);
            }
            catch 
            {
            }
        }

        internal void ManualOpen()
        {
            if (Manual == null)
            {
                Manual = new ManualUI();
            }

            var desktop = System.Windows.SystemParameters.WorkArea;
            Manual.Left = desktop.Width - Manual.Width;
            Manual.Top = desktop.Bottom - Manual.Height;
            Manual.Show();
        }

        internal void ManualClose()
        {
            if (Manual != null && Manual.IsVisible)
            {
                Manual.Close(); 
            }
        }
    }
}
