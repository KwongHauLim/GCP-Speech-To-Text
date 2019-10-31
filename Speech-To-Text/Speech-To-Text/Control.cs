using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HaLi.Tools.Encryption;
using Newtonsoft.Json;

namespace Speech_To_Text
{
    public class Control
    {
        private static Control _ptr;
        public static Control Share => _ptr = _ptr ?? new Control();

        private readonly string path = "Setting.json";
        public Setting Setting { get; set; }

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
    }
}
