using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace HaLi.AudioInput
{
    public class Microphone
    {
        private static Microphone _ptr = null;
        public static Microphone Share => _ptr = _ptr ?? new Microphone();

        public WaveInEvent WaveIn { get; private set; }
        public WaveFileWriter Writer { get; private set; }

        private string tempFile;
        /// <summary>
        /// 正在收音
        /// </summary>
        public static bool IsRecording { get; set; }
        /// <summary>
        /// 已寫入音頻長度
        /// </summary>
        public static double Length { get; private set; }
        /// <summary>
        /// 錄音中當刻測量音量
        /// </summary>
        public static float Volume { get; private set; }
        private bool done = false;

        /// <summary>
        /// 是否寫入檔案
        /// </summary>
        public bool WriteData { get; set; } = true;

        public Action<WaveInEventArgs> OnReceive { private get; set; }

        private object locker = new object();

        private Microphone()
        {
            WaveIn = new WaveInEvent
            {
                DeviceNumber = 0,
                WaveFormat = new WaveFormat(16000, 1)
            };
            WaveIn.DataAvailable += (sender, args) =>
            {
                lock (locker)
                {
                    if (Writer != null)
                    {
                        float max = 0;
                        // interpret as 16 bit audio
                        for (int index = 0; index < args.BytesRecorded; index += 2)
                        {
                            short sample = (short)((args.Buffer[index + 1] << 8) |
                                                    args.Buffer[index + 0]);
                            // to floating point
                            var sample32 = sample / 32768f;
                            // absolute value 
                            if (sample32 < 0) sample32 = -sample32;
                            // is this the max value?
                            if (sample32 > max) max = sample32;
                        }
                        Volume = max;

                        if (OnReceive != null)
                            OnReceive(args);

                        if (WriteData)
                        {
                            Writer.Write(args.Buffer, 0, args.BytesRecorded);
                            Writer.Flush();
                            Length = (double)Writer.Position / WaveIn.WaveFormat.AverageBytesPerSecond;
                        }
                    } 
                }
            };
        }

        public static void StartRecording() => Share.Start();
        private Task Start()
        {
            IsRecording = true;
            //if (!IsRecording)
            {
                tempFile = Path.GetTempFileName();
                File.WriteAllBytes(tempFile, new byte[0]);

                ChangeFile();

                IsRecording = true;
                done = false;

                return Task.Run(() =>
                {
                    WaveIn.StartRecording();

                    while (IsRecording) Thread.Sleep(0);

                    WaveIn.StopRecording();
                    done = true;
                }); 
            }
            //return null;
        }

        public static void StopRecording() => Share.Stop().Wait();
        private Task Stop()
        {
            return Task.Run(() =>
            {
                while (!done)
                {
                    IsRecording = false;
                    Thread.Sleep(0);
                }
            });
        }

        public static byte[] GetData()
        {
            return null;
        }

        public static void WriteToFile(string path)
        {
            lock (Share.locker)
            {
                try
                {
                    if (File.Exists(Share.tempFile))
                        File.Copy(Share.tempFile, path);
                }
                catch { }
            }
        }

        public void ChangeFile()
        {
            lock (locker)
            {
                if (Writer != null)
                {
                    Writer.Flush();
                    Writer.Close();
                    Writer.Dispose();
                    Writer = null;
                }

                try
                {
                    File.Delete(tempFile); 
                }
                catch { }

                Writer = new WaveFileWriter(tempFile, WaveIn.WaveFormat);
                Length = 0; 
            }
        }
    }
}
