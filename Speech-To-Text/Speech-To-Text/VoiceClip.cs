using System;
using System.Threading.Tasks;
using HaLi.GoogleSpeech;
using RecordMode = Speech_To_Text.Setting.GoogleSpeech.RecordMode;

namespace Speech_To_Text
{
    // not in use
    public class VoiceClip
    {
        public SpeechTask Speech { get; private set; }
        public Task<SpeechData> Task { get; private set; }
        public SpeechData Result { get; private set; }

        public void StartVoice(RecordMode mode, string lang)
        {
            Speech = new SpeechTask();
            Speech.Language = lang;
            Speech.KeepWavFile = $@"R:\Voice{DateTime.Now.ToString("mmss")}.wav";

            var setting = Control.Share.Setting;
            var min = setting.MinLength;
            var max = setting.MaxLength;

            if (mode == RecordMode.File)
                Task = Speech.StartRecord(min, max);
            else if (mode == RecordMode.Stream)
                Task = Speech.StartStream();
        }

        public SpeechData StopVoice()
        {
            if (Speech != null && Task != null)
            {
                Speech.StopRecording();
                try
                {
                    Result = Task.Result;
                    return Result;
                }
                catch { }
            }
            return null;
        }
    }
}
