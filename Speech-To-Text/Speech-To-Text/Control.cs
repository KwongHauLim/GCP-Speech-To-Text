using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using HaLi.AudioInput;
using HaLi.GoogleSpeech;
using HaLi.Tools.Encryption;
using Newtonsoft.Json;
using Speech_To_Text.View.Manual;
using RecordMode = Speech_To_Text.Setting.GoogleSpeech.RecordMode;
using SensitiveMode = Speech_To_Text.Setting.GoogleSpeech.SensitiveMode;

namespace Speech_To_Text
{
    public class Control
    {
        private static Control _ptr;
        public static Control Share => _ptr = _ptr ?? new Control();

        private readonly string path = "Setting.json";
        public Setting Setting { get; set; }

        public ManualUI Manual { get; private set; }
        public SpeechTask Speech { get; private set; }
        public Task<SpeechData> Task { get; private set; }

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

        public void ManualOpen()
        {
            if (Manual == null)
            {
                var desktop = System.Windows.SystemParameters.WorkArea;
                Manual = new ManualUI();
                Manual.Show();
                Manual.Left = desktop.Width - Manual.Width;
                Manual.Top = desktop.Bottom - Manual.Height; 
            }
        }

        public void ManualClose()
        {
            if (Manual != null && Manual.IsVisible)
            {
                Manual.Close();
                Manual = null;
            }
        }

        public void StartVoice()
        {
            Speech = new SpeechTask();
            Speech.Language = "en";

            if (Setting.Speech.Mode == RecordMode.File)
                Task = Speech.StartRecord(0);
            else if (Setting.Speech.Mode == RecordMode.Stream)
                Task = Speech.StartStream();
        }

        public void StopVoice()
        {
            if (Speech != null && Task != null)
            {
                Microphone.StopRecording();
                var data = Task.Result;
            }
        }
    }
}
