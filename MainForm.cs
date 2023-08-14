using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KirbyHitboxViewer
{
    public partial class MainForm : Form
    {
        public bool isFillRectangle = false;

        bool isDrawing;
        Point startPoint;

        public List<Rectangle> rects = new List<Rectangle>();
        List<Rectangle> storePreviousList = new List<Rectangle>();

        public static MainForm instance;

        public MainForm()
        {
            InitializeComponent();
            this.CenterToScreen();
            ApplySettings();
            Label testLabel = new Label();
            testLabel.Text = "Test";

            if(instance != null )
            {
                MessageBox.Show("There is more than one instance of MainForm, the application will close. Try reinstalling this application.", "Error : could not load application", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            instance = this;
        }

        public void ApplySettings()
        {
            string iniFilePath = Path.Combine(Application.StartupPath, "config.ini");
            SettingsIniFile ini = new SettingsIniFile(iniFilePath);
            if (ini.ReadValue("Main", "IsFillRectangle") == "True")
            {
                isFillRectangle = true;
            }
            else
            {
                isFillRectangle = false;
            }
            this.Refresh();
        }

        void DrawEditor(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.LightGray);

            int cellWidth = 40; // Width of each cell in the grid
            int cellHeight = 40; // Height of each cell in the grid

            // Draw horizontal lines
            for (int y = 8; y < this.Height; y += cellHeight)
            {
                g.DrawLine(pen, 0, y, this.Width, y);
            }

            // Draw vertical lines
            for (int x = 18; x < this.Width; x += cellWidth)
            {
                g.DrawLine(pen, x, 0, x, this.Height);
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            DrawEditor(e);

            if(rects.Count != 0)
            {
                foreach(Rectangle rect in rects)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Red, 2f), rect);

                    if (isFillRectangle)
                    {
                        Brush brush = new SolidBrush(Color.FromArgb(110, 255, 0, 0));
                        e.Graphics.FillRectangle(brush, rect);
                    }
                }
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left && !isDrawing)
            {
                if(rects.Count < 10)
                {
                    startPoint = e.Location;
                    rects.Add(new Rectangle());
                    isDrawing = true;
                    storePreviousList = new List<Rectangle>(rects);
                    storePreviousList.RemoveAt(storePreviousList.Count - 1);
                }
                else
                {
                    MessageBox.Show("For performance purposes, you cannot have more than 10 hitboxes.", "Sorry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if(isDrawing)
            {
                if(e.X >= startPoint.X && e.Y < startPoint.Y)
                {
                    //Up right
                    rects[rects.Count - 1] = new Rectangle(startPoint.X, e.Y, e.X - startPoint.X, startPoint.Y - e.Y);
                }
                else if(e.X >= startPoint.X && e.Y >= startPoint.Y)
                {
                    //Down right
                    rects[rects.Count - 1] = new Rectangle(startPoint.X, startPoint.Y, e.X - startPoint.X, e.Y - startPoint.Y);
                }
                else if(e.X < startPoint.X && e.Y < startPoint.Y)
                {
                    //Up left
                    rects[rects.Count - 1] = new Rectangle(e.X, e.Y, startPoint.X - e.X, startPoint.Y - e.Y);
                }
                else
                {
                    //Down left
                    rects[rects.Count - 1] = new Rectangle(e.X, startPoint.Y, startPoint.X - e.X, e.Y - startPoint.Y);
                }
                
                this.Refresh();
            }
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                isDrawing = false;
                this.Refresh();
            }
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            storePreviousList = new List<Rectangle>(rects);
            rects.Clear();
            this.Refresh();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (storePreviousList.Count == rects.Count)
            {
                MessageBox.Show("Nothing to undo!", "Kirby Hitbox Viewer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            rects = new List<Rectangle>(storePreviousList);

            if(storePreviousList.Count > 0)
            {
                storePreviousList.RemoveAt(storePreviousList.Count - 1);
            }

            this.Refresh();
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Preferences newForm = new Preferences();
            newForm.ShowDialog();
        }

        private void showValuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(rects.Count > 0)
            {
                ResultValues form = new ResultValues();
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("No hitbox!", "Couldn't load results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About form = new About();
            form.ShowDialog();
        }
    }

    public class SettingsIniFile
    {
        string path;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);

        public SettingsIniFile(string INIPath)
        {
            path = INIPath;
        }

        public void WriteValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, path);
        }

        public string ReadValue(string section, string key)
        {
            var retVal = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", retVal, 255, path);
            return retVal.ToString();
        }
    }
}
