using System;
using System.Collections.Generic;
using System.Text;

namespace Speech_To_Text
{
    public class Setting
    {
        /// <summary>
        /// APP啟動隨即開始錄音
        /// </summary>
        public bool EnableWhenStart { get; set; } = true;
        /// <summary>
        /// APP啟動時設定識別語言
        /// </summary>
        public string DefaultLanguage { get; set; } = "en";

        public double MinLength { get; set; } = 0.0;
        public double MaxLength { get; set; } = 60.0;
        /// <summary>
        /// 每x秒寫成一個wav檔傳到Google識別
        /// </summary>
        public double ClipLength { get; set; } = 5.0;

        /// <summary>
        /// wav檔傳到Google後是否保留
        /// </summary>
        public bool KeepWavFile { get; set; } = true;
        /// <summary>
        /// Exit制, 刪除所有wav檔
        /// </summary>
        public bool DeleteWhenExit { get; set; } = true;

        public class GoogleSpeech
        {
            public enum RecordMode
            {
                None = 0,
                File = 1,
                Stream = 2,
            }

            // 以音量判定開始時間
            // Fail, 開始終結時間難控制
            //public enum SensitiveMode
            //{
            //    None = 0,
            //    Manual = 1,
            //    Low = 2,
            //    High = 3,
            //}

            public string Credential { get; set; } = string.Empty;
            public RecordMode Mode { get; set; } = RecordMode.File;
            //public SensitiveMode Sensitive { get; set; } = SensitiveMode.High;
        }
        public GoogleSpeech Speech { get; set; } = new GoogleSpeech();
    }
}
