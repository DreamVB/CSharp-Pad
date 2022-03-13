using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DreamVB.Config;

namespace CSharpPad
{
    class tools
    {
        public static string CompilerPath { get; set; }

        public static string QString(string S)
        {
            return "\"" + S + "\"";
        }

        public static string FixPath(string Path)
        {
            if (!Path.EndsWith("\\"))
            {
                return Path + "\\";
            }
            return Path;
        }

        public static void LoadCompilerPath()
        {
            cfgfile cfg = new cfgfile("config.ini");
            CompilerPath = cfg.ReadString("main", "compiler");
        }

        public static void SaveCompilerPath()
        {
            cfgfile cfg = new cfgfile();
            cfg.WriteString("main", "compiler", CompilerPath);
            cfg.SaveIniFile("config.ini");
        }

    }
}
