using System;
using System.Collections.Generic;
using System.Text;

namespace HaLi.GoogleSpeech
{
    public class SpeechData
    {
        public DateTime Time { get; set; } = DateTime.Now;
        public string WavFile { get; set; } = string.Empty;
        public TimeSpan Length { get; set; } = TimeSpan.Zero;
        public string Text { get; set; } = string.Empty;
    }
}
