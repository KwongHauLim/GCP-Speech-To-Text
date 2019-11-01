﻿using System;
using System.Threading.Tasks;
using HaLi.GoogleSpeech;
using RecordMode = Speech_To_Text.Setting.GoogleSpeech.RecordMode;

namespace Speech_To_Text
{
    public class VoiceClip
    {
        public SpeechTask Speech { get; private set; }
        public Task<SpeechData> Task { get; private set; }
        public SpeechData Result { get; private set; }

        public void StartVoice(RecordMode mode)
        {
            Speech = new SpeechTask();
            Speech.Language = "en";
            Speech.KeepWavFile = $@"R:\Voice{DateTime.Now.ToString("mmss")}.wav";

            if (mode == RecordMode.File)
                Task = Speech.StartRecord(0);
            else if (mode == RecordMode.Stream)
                Task = Speech.StartStream();
        }

        public SpeechData StopVoice()
        {
            if (Speech != null && Task != null)
            {
                Speech.StopRecording();
                Result = Task.Result;
            }
            return Result;
        }
    }
}