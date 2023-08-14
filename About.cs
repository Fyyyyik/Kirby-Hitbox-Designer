using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KirbyHitboxViewer
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
            this.Refresh();
        }

        private void About_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Black, 2f);
            e.Graphics.DrawRectangle(pen, new Rectangle(panel1.Location.X, panel1.Location.Y, panel1.Size.Width, panel1.Size.Height));
            e.Graphics.DrawRectangle(pen, new Rectangle(panel2.Location.X, panel2.Location.Y, panel2.Size.Width, panel2.Size.Height));
        }
    }
}
