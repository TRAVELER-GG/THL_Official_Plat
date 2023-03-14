using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace League.Plat
{
    public static class Web_Root
    {
        public static readonly string wwwroot = Igrone_Replace();
        public static readonly string Upload_File = "Upload_File";
        private static string Igrone_Replace()
        {
            string Root = AppContext.BaseDirectory;
            Root = Root.Replace("bin\\Debug\\netcoreapp3.1\\", "");
            Root = Root.Replace("bin\\Release\\netcoreapp3.1\\", "");
            Root += "wwwroot" + "\\";
            return Root;
        }
    }
}
