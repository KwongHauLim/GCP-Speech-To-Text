using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Cloud.Speech.V1;
using NAudio.Wave;
using HaLi.AudioInput;
using SpeechStream = Google.Cloud.Speech.V1.SpeechClient.StreamingRecognizeStream;

namespace HaLi.GoogleSpeech
{
    public class SpeechTask
    {
        public string Language { get; set; } = "en";
        public string KeepWavFile { get; set; } = string.Empty;

        private object locker = new object();
        private StringBuilder sb;
        public string StreamingText
        {
            get
            {
                string text = string.Empty;
                lock (locker)
                {
                    if (sb != null && sb.Length > 0)
                    {
                        text = sb.ToString();
                        sb.Length = 0; 
                    }
                }
                return text;
            }
        }

        /// <summary>
        /// 完整音檔, 發到Google轉文字
        /// </summary>
        public static Task<SpeechData> FromFile(string path, string Language)
        {
            return Task.Run(() =>
            {
                var sb = new StringBuilder();

                GCP.Share.Init();
                var response = GCP.Share.Client.Recognize(new RecognitionConfig()
                {
                    LanguageCode = Language,
                }, RecognitionAudio.FromFile(path));

                foreach (var result in response.Results)
                {
                    foreach (var alternative in result.Alternatives)
                    {
                        sb.Append(alternative.Transcript);
                    }
                }

                return new SpeechData
                {
                    WavFile = path,
                    Length = TimeSpan.FromMilliseconds(SoundInfo.GetSoundLength(path)),
                    Text = sb.ToString()
                };
            });
        }

        /// <summary>
        /// 錄音後, 發到Google轉文字
        /// </summary>
        public Task<SpeechData> StartRecord(double minimum, double maximum)
        {
            return Task.Run(() =>
            {
                Microphone.StartRecording();

                while (Microphone.IsRecording)
                {
                    if (Microphone.Length.CompareTo(maximum) >= 0)
                        Microphone.StopRecording();
                    Thread.Sleep(1);
                }

                if (Microphone.Length.CompareTo(minimum) < 0)
                    return null;

                var path = KeepWavFile;
                if (string.IsNullOrWhiteSpace(path))
                    path = Path.GetTempFileName();

                Microphone.WriteToFile(path);
                path = Path.ChangeExtension(path, ".wav");

                return FromFile(path, Language);
            });
        }

        /// <summary>
        /// 邊錄音, 邊發到Google轉文字
        /// </summary>
        public Task<SpeechData> StartStream()
        {
            return Task.Run(() =>
            {
                GCP.Share.Init();
                var StreamingConfig = new StreamingRecognitionConfig()
                {
                    Config = new RecognitionConfig()
                    {
                        Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                        SampleRateHertz = 16000,
                        LanguageCode = Language,
                    },
                    InterimResults = true,
                };
                var Streaming = GCP.Share.Client.StreamingRecognize();
                Streaming.WriteAsync(new StreamingRecognizeRequest { StreamingConfig = StreamingConfig }).Wait();

                Microphone.Share.OnReceive = (args) =>
                {
                    if (Streaming != null)
                    {
                        try
                        {
                            Streaming.WriteAsync(
                                new StreamingRecognizeRequest()
                                {
                                    AudioContent = Google.Protobuf.ByteString.CopyFrom(args.Buffer, 0, args.BytesRecorded)
                                }).Wait();
                        }
                        catch (Exception ex)
                        {

                            throw ex;
                        }
                    }
                };

                var speechAnaysis = Task.Run<string>(async () =>
                {
                    sb = new StringBuilder();

                    try
                    {
                        while (await Streaming.ResponseStream.MoveNext(default))
                        {
                            try
                            {
                                foreach (var result in Streaming.ResponseStream.Current.Results)
                                {
                                    foreach (var alternative in result.Alternatives)
                                    {
                                        lock (locker)
                                        {
                                            sb.Append(alternative.Transcript);
                                        }
                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    catch { }

                    return sb.ToString();
                });

                Microphone.StartRecording();

                Streaming.WriteCompleteAsync().Wait();

                if (!string.IsNullOrWhiteSpace(KeepWavFile))
                    Microphone.WriteToFile(KeepWavFile);

                return new SpeechData
                {
                    WavFile = KeepWavFile,
                    Length = TimeSpan.FromSeconds(Microphone.Length),
                    Text = speechAnaysis.Result
                };
            });
        }

        public void StopRecording()
        {
            Microphone.StopRecording();
        }
    }
}
