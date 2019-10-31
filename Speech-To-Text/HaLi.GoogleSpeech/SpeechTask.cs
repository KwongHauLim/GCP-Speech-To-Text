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
        public bool KeepWavFile { get; set; } = true;
                
        /// <summary>
        /// 完整音檔, 發到Google轉文字
        /// </summary>
        public Task<SpeechData> FromFile(string path)
        {
            return Task.Run(() =>
            {
                var sb = new StringBuilder();

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
        public Task<SpeechData> StartRecord(int length)
        {
            return Task.Run(() =>
            {
                Microphone.StartRecording();

                if (length > 0)
                {
                    Thread.Sleep(length);
                    Microphone.StopRecording();
                }

                return FromFile(KeepFile(true));
            });
        }

        /// <summary>
        /// 邊錄音, 邊發到Google轉文字
        /// </summary>
        public Task<SpeechData> StartStream()
        {
            return Task.Run(() =>
            {
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
                    var sb = new StringBuilder();

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
                                        sb.Append(alternative.Transcript);
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

                return new SpeechData
                {
                    WavFile = KeepFile(KeepWavFile),
                    Length = TimeSpan.FromSeconds(Microphone.GetLength()),
                    Text = speechAnaysis.Result
                };
            });
        }

        private string KeepFile(bool keep)
        {
            if (keep)
            {
                string path = @$"R:\voice{DateTime.Now.ToString("mmss")}.wav";
                Microphone.WriteToFile(path);
                return path;
            }
            return string.Empty;
        }
    }
}
