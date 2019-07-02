using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZiePlayer
{
    class TrackList
    {
        public static string AppPath = AppDomain.CurrentDomain.BaseDirectory; // Путь к исполняемуому файлу


        public static List<string> Files = new List<string>();

        public static string GetFileName(string file)
        {
            string[] tmp = file.Split('\\');
                 
            return tmp[tmp.Length - 1];
        }
       
    }
}
