using System;
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
        public bool IsRecording { get; set; }
        public double Length { get; private set; }
        public float Volume { get; private set; }
        private bool done = false;

        public static float MicVolume => Share.Volume;

        public Action<WaveInEventArgs> OnReceive { private get; set; }

        private Microphone()
        {
            WaveIn = new WaveInEvent
            {
                DeviceNumber = 0,
                WaveFormat = new WaveFormat(16000, 1)
            };
            WaveIn.DataAvailable += (sender, args) =>
            {
                if (Writer != null)
                {
                    Writer.Write(args.Buffer, 0, args.BytesRecorded);
                    Writer.Flush();
                    Length = (double)Writer.Position / WaveIn.WaveFormat.AverageBytesPerSecond;

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
                }

                if (OnReceive != null)
                    OnReceive(args);
            };
        }

        public static void StartRecording() => Share.Start();
        private Task Start()
        {
            tempFile = Path.GetTempFileName();
            File.WriteAllBytes(tempFile, new byte[0]);

            Writer = new WaveFileWriter(tempFile, WaveIn.WaveFormat);

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

        public static void StopRecording() => Share.Stop().Wait();
        private Task Stop()
        {
            IsRecording = false;
            return Task.Run(() =>
            {
                while (!done) Thread.Sleep(0);
            });
        }

        public static double GetLength() => Share.Length;

        public static byte[] GetData()
        {
            return null;
        }

        public static void WriteToFile(string path)
        {
            if (File.Exists(Share.tempFile))
                File.Copy(Share.tempFile, path);
        }
    }
}
