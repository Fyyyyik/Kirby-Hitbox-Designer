using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KirbyHitboxViewer
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            LoadSettings();
            Application.Run(new MainForm());
        }

        static void LoadSettings()
        {
            string iniFilePath = Path.Combine(Application.StartupPath, "config.ini");

            if(File.Exists(iniFilePath) == false)
            {
                SettingsIniFile iniFile = new SettingsIniFile(iniFilePath);
                iniFile.WriteValue("DebugSection", "DebugKey", "DebugValue");
                iniFile.WriteValue("Main", "IsFillRectangle", "False");
            }
        }
    }
}
