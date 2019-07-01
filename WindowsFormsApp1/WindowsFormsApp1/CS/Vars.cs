using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
   public static class Vars
    {
        public static List<string> Files = new List<string>(); // Список полных имен файлов


        public static string GetFileName(string file)
        {
            string[] tmp = file.Split('\\');
            return tmp[tmp.Length-1];
        }
    }
}
