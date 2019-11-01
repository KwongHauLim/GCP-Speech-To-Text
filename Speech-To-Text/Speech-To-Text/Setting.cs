using System;
using System.Collections.Generic;
using System.Text;

namespace Speech_To_Text
{
    public class Setting
    {
        public bool EnableWhenStart { get; set; } = true;
        public string DefaultLanguage { get; set; } = "en";

        public double MinLength { get; set; } = 0.0;
        public double MaxLength { get; set; } = 60.0;

        public bool KeepWavFile { get; set; } = true;
        public bool DeleteWhenExit { get; set; } = true;

        public class GoogleSpeech
        {
            public enum RecordMode
            {
                None = 0,
                File = 1,
                Stream = 2,
            }

            public enum SensitiveMode
            {
                None = 0,
                Manual = 1,
                Low = 2,
                High = 3,
            }

            public string Credential { get; set; } = string.Empty;
            public RecordMode Mode { get; set; } = RecordMode.File;
            public SensitiveMode Sensitive { get; set; } = SensitiveMode.High;
        }
        public GoogleSpeech Speech { get; set; } = new GoogleSpeech();
    }
}
