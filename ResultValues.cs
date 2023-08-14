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
    public partial class ResultValues : Form
    {
        List<Rectangle> panelOutlines = new List<Rectangle>();

        public ResultValues()
        {
            InitializeComponent();
        }

        string FloatToHex(float input)
        {
            byte[] bytes = BitConverter.GetBytes(input);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            string output = BitConverter.ToString(bytes).Replace("-", "");
            return output;
        }

        private void ResultValues_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
            int pixelYOffset = 68;
            int loopCount = 1;
            bool isFirstLoop = true;
            Panel previousPanel = null;
            Label previousLabel = null;
            TextBox previousWidthValue = null;
            TextBox previousHeightValue = null;
            TextBox previousXValue = null;
            TextBox previousYValue = null;
            Label previousWidthLabel = null;
            Label previousHeightLabel = null;
            Label previousXLabel = null;
            Label previousYLabel = null;
            List<Rectangle> results = new List<Rectangle>(MainForm.instance.rects);
            foreach (Rectangle r in results)
            {
                //Panel creation
                Panel panel = new Panel();
                if (isFirstLoop)
                {
                    panel.Location = new Point(12, 12);
                }
                else
                {
                    panel.Location = new Point(12, previousPanel.Location.Y + pixelYOffset);
                }
                panel.Size = new Size(416, 65);
                previousPanel = panel;
                panel.SendToBack();
                //Draw panel outline
                panelOutlines.Add(new Rectangle(panel.Location.X, panel.Location.Y, panel.Size.Width, panel.Size.Height));

                //Hitbox Label
                Label label = new Label();
                if (isFirstLoop)
                {
                    label.Location = new Point(28, 15);
                }
                else
                {
                    label.Location = new Point(28, previousLabel.Location.Y + pixelYOffset);
                }
                label.AutoSize = true;
                label.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular);
                label.Text = "Hitbox " + loopCount;
                previousLabel = label;
                Controls.Add(label);

                //Hitbox width text
                TextBox widthValue = new TextBox();
                if (isFirstLoop)
                {
                    widthValue.Location = new Point(54, 50);
                }
                else
                {
                    widthValue.Location = new Point(54, previousWidthValue.Location.Y + pixelYOffset);
                }
                widthValue.ReadOnly = true;
                widthValue.Size = new Size(77, 20);
                widthValue.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular);
                widthValue.Text = "0x" + FloatToHex((float)results[loopCount - 1].Width / 40);
                previousWidthValue = widthValue;
                Controls.Add(widthValue);

                //Hitbox height text
                TextBox heightValue = new TextBox();
                if (isFirstLoop)
                {
                    heightValue.Location = new Point(137, 50);
                }
                else
                {
                    heightValue.Location = new Point(137, previousHeightValue.Location.Y + pixelYOffset);
                }
                heightValue.ReadOnly = true;
                heightValue.Size = new Size(77, 20);
                heightValue.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular);
                heightValue.Text = "0x" + FloatToHex((float)results[loopCount - 1].Height / 40);
                previousHeightValue = heightValue;
                Controls.Add(heightValue);

                //Hitbox X text
                TextBox xValue = new TextBox();
                int x = heightValue.Location.X + (heightValue.Location.X - widthValue.Location.X);
                if (isFirstLoop)
                {
                    xValue.Location = new Point(x, 50);
                }
                else
                {
                    xValue.Location = new Point(x, previousXValue.Location.Y + pixelYOffset);
                }
                xValue.ReadOnly = true;
                xValue.Size = new Size(77, 20);
                xValue.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular);
                xValue.Text = "0x" + FloatToHex((((float)results[loopCount - 1].Location.X + (float)results[loopCount - 1].Size.Width / 2f) - ((float)MainForm.instance.Size.Width / 2f - 9f)) / 40f);
                previousXValue = xValue;
                Controls.Add(xValue);

                //Hitbox Y text
                TextBox yValue = new TextBox();
                x = xValue.Location.X + (xValue.Location.X - heightValue.Location.X);
                if (isFirstLoop)
                {
                    yValue.Location = new Point(x, 50);
                    isFirstLoop = false;
                }
                else
                {
                    yValue.Location = new Point(x, previousYValue.Location.Y + pixelYOffset);
                }
                yValue.ReadOnly = true;
                yValue.Size = new Size(77, 20);
                yValue.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular);
                yValue.Text = "0x" + FloatToHex((((float)results[loopCount - 1].Location.Y + (float)results[loopCount - 1].Size.Height / 2f) - ((float)MainForm.instance.Size.Height / 2f + 10.8f)) / 40f);
                previousYValue = yValue;
                Controls.Add(yValue);

                //Label width
                Label widthLabel = new Label();
                widthLabel.Location = new Point(widthValue.Location.X, widthValue.Location.Y - 15);
                widthLabel.AutoSize = true;
                widthLabel.Font = new Font("Microsoft Sans Serif", 8.5f, FontStyle.Regular);
                widthLabel.Text = "Width";
                Controls.Add(widthLabel);

                //Label height
                Label heightLabel = new Label();
                heightLabel.Location = new Point(heightValue.Location.X, heightValue.Location.Y - 15);
                heightLabel.AutoSize = true;
                heightLabel.Font = new Font("Microsoft Sans Serif", 8.5f, FontStyle.Regular);
                heightLabel.Text = "Height";
                Controls.Add(heightLabel);

                //Label X
                Label xLabel = new Label();
                xLabel.Location = new Point(xValue.Location.X, xValue.Location.Y - 15);
                xLabel.AutoSize = true;
                xLabel.Font = new Font("Microsoft Sans Serif", 8.5f, FontStyle.Regular);
                xLabel.Text = "X";
                Controls.Add(xLabel);

                //Label Y
                Label yLabel = new Label();
                yLabel.Location = new Point(yValue.Location.X, yValue.Location.Y - 15);
                yLabel.AutoSize = true;
                yLabel.Font = new Font("Microsoft Sans Serif", 8.5f, FontStyle.Regular);
                yLabel.Text = "Y";
                Controls.Add(yLabel);

                loopCount++;
            }
            this.Refresh();
        }

        private void ResultValues_Paint(object sender, PaintEventArgs e)
        {
            foreach (Rectangle r in panelOutlines)
            {
                e.Graphics.DrawRectangle(new Pen(Color.Black, 2f), r);
            }
        }
    }
}
