using System.Collections.Generic;
using Un4seen.Bass.AddOn.Tags;

namespace EZiePlayer
{
    class TagModel
    {
        public int Freq;
        public int BitRate;
        public string Channels;
        public string Artist;
        public string Album;
        public string Title;
        public string Year;

        Dictionary<int, string> channels = new Dictionary<int, string>()
        {
            {0, "Null" },
            {1, "Mono" },
            {2, "Stereo" }
        };

        public TagModel(string file)
        {
            TAG_INFO tagInfo = new TAG_INFO();
            tagInfo = BassTags.BASS_TAG_GetFromFile(file);
            BitRate = tagInfo.bitrate;
            Freq = tagInfo.channelinfo.freq;
            Channels = channels[tagInfo.channelinfo.chans];
            Artist = tagInfo.artist;
            Album = tagInfo.album;
      
           

            if (tagInfo.title == "")
            {
                Title = TrackList.GetFileName(file);
               
            }
                
            else Title = tagInfo.title; Year = tagInfo.year;

        }

    }
}
