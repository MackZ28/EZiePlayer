using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Un4seen.Bass;

namespace WindowsFormsApp1
{
    public static class BassLike
    {
     
        private static int HZ = 44100; // частота дискритизации

        public static bool InitDefaultDevice; // Состояние инициализации

        public static int Stream; // Канал(Поток)

        public static int Volume = 100; //Громкость


        private static bool InitBass(int hz) // Инициализация Bass.dll
        {
            if (!InitDefaultDevice)
                InitDefaultDevice = Bass.BASS_Init(-1, hz, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
            return InitDefaultDevice;
        }

        public static void Play(string filename, int vol) //Воспроизведение
        {
            Stop();
            if (InitBass(HZ))
            {
                Stream = Bass.BASS_StreamCreateFile(filename, 0, 0, BASSFlag.BASS_DEFAULT);

                if (Stream != 0)
                {
                    Volume = vol;
                    Bass.BASS_ChannelSetAttribute(Stream, BASSAttribute.BASS_ATTRIB_VOL, Volume / 100);
                    Bass.BASS_ChannelPlay(Stream, false);
                }

            }
        }


        public static void Stop()
        {
            Bass.BASS_ChannelStop(Stream);
            Bass.BASS_StreamFree(Stream);
        }

        public static void Pause()
        {
            Bass.BASS_ChannelPause(Stream);

        }


        public static int GetTimeOfStream(int stream) // Длительность канала в секундах
        {
            long TimeBytes = Bass.BASS_ChannelGetLength(stream);
            double Time = Bass.BASS_ChannelBytes2Seconds(stream, TimeBytes);
                return (int)Time;
        }


        public static int GetPosOfStream(int stream) // Получение текущей позиции в секундах
        {
            long pos = Bass.BASS_ChannelGetPosition(stream);
            int posSec = (int)Bass.BASS_ChannelBytes2Seconds(stream, pos);
            return posSec;
        }


        public static void SetPosOfScroll(int stream,int pos)
        {
            Bass.BASS_ChannelSetPosition(stream, (double)pos);
        }



        public static void SetVolumeToStream(int stream, int vol) //Установка громкости
        {
            Volume = vol;
            Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, Volume / 100F);

        }
       

    }
}
