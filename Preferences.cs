using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KirbyHitboxViewer
{
    public partial class Preferences : Form
    {
        public Preferences()
        {
            InitializeComponent();
        }

        private void Preferences_Load(object sender, EventArgs e)
        {
            this.CenterToParent();

            if (MainForm.instance.isFillRectangle)
            {
                checkBox1.Checked = true;
            }
        }

        private void btn_apply_Click(object sender, EventArgs e)
        {
            string iniFilePath = Path.Combine(Application.StartupPath, "config.ini");
            SettingsIniFile ini = new SettingsIniFile(iniFilePath);
            if (checkBox1.Checked)
            {
                ini.WriteValue("Main", "IsFillRectangle", "True");
            }
            else
            {
                ini.WriteValue("Main", "IsFillRectangle", "False");
            }
            MainForm.instance.ApplySettings();
        }
    }
}
