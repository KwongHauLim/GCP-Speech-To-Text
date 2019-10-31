﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Speech_To_Text
{
    public class Setting
    {
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