using System;
using Un4seen.Bass;

namespace EZiePlayer
{
    class Playerlogic
    {
        public static int HZ = 44100; // частота дискритизации

        public static bool InitDefaultDevice; // Состояние инициализации

        public static int Stream; // Канал(Поток)

        public int Volume = 100; //Громкость



        public bool InitBass(int hz) // Инициализация Bass.dll
        {
            if (!InitDefaultDevice)
            {
                InitDefaultDevice = Bass.BASS_Init(-1, hz, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
            }
            return InitDefaultDevice;
        }
        
        public void Play(string filename, int vol) //Воспроизведение
        {
            if (Bass.BASS_ChannelIsActive(Stream) != BASSActive.BASS_ACTIVE_PAUSED)
            {
                Stop();
                if (InitBass(HZ))
                {
                    Stream = Bass.BASS_StreamCreateFile(filename, 0, 0, BASSFlag.BASS_DEFAULT);

                    if (Stream != 0)
                    {
                        Volume = vol;
                        Bass.BASS_ChannelSetAttribute(Stream, BASSAttribute.BASS_ATTRIB_VOL, Volume / 100F);
                        Bass.BASS_ChannelPlay(Stream, false);
                    }
                }
            }
            else Bass.BASS_ChannelPlay(Stream, false);
        }
        public void Stop()
        {
            Bass.BASS_ChannelStop(Stream);
            Bass.BASS_StreamFree(Stream);
        }
        public void SetVolumeToStream(int stream, int vol) //Установка громкости
        {
            Volume = vol;
            Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, Volume / 100F);
        }

        public void Pause()
        {
            if (Bass.BASS_ChannelIsActive(Stream) == BASSActive.BASS_ACTIVE_PLAYING)
                Bass.BASS_ChannelPause(Stream);
        }

        public int GetPosOfStream(int stream) // Получение текущей позиции в секундах
        {
            long pos = Bass.BASS_ChannelGetPosition(stream);
            return (int)Bass.BASS_ChannelBytes2Seconds(stream, pos);
        }

        public int GetTimeOfStream(int stream)
        {
            long Bytes = Bass.BASS_ChannelGetLength(stream);
            double Time = Bass.BASS_ChannelBytes2Seconds(stream, Bytes); 
            return (int)Time;
        }

        public static void GetPosOfScroll(int stream, double pos)
        {
          Bass.BASS_ChannelSetPosition(stream, (long)pos);
        }
        
        public string GetLengthOfFile(string filename)
        {
          string length = TimeSpan.FromSeconds(GetTimeOfStream(Bass.BASS_StreamCreateFile(filename, 0, 0, BASSFlag.BASS_DEFAULT))).ToString();
          return length;
        }
    }
}
