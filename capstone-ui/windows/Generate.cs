using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace capstone_ui.windows
{
    public partial class Generate : Form
    {
        public string filePath;

        public Generate(string filePath)
        {
            InitializeComponent();
        }


        private void Generate_Load(object sender, EventArgs e)
        {
            Width = 326;
            Height = 127;

            button2.Location = new Point(142, 89);

            button3.Location = new Point(223, 89);

            disableAllControls();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();

            if(open.ShowDialog() == DialogResult.OK)
            {
                
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
            {
                disableAllControls();

                enableStandardMatrixControls();

            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton2.Checked)
            {
                disableAllControls();

                enableSpecialMatrixControls();
            }
        }

        private void disableAllControls()
        {
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;

            radioButton3.Visible = false;
            radioButton4.Visible = false;
            radioButton5.Visible = false;
            radioButton6.Visible = false;
            radioButton7.Visible = false;
            radioButton8.Visible = false;

            trackBar1.Visible = false;

            button4.Visible = false;

            textBox2.Visible = false;
        }

        private void enableStandardMatrixControls()
        {

        }

        private void enableSpecialMatrixControls()
        {

        }

        private void Generate_SizeChanged(object sender, EventArgs e)
        {
            //Create New point to represent button2's location
            Point point = new Point(button2.Location.X, Height - 68);

            //Set button2 location equal to point
            button2.Location = point;

            //Create new point to represent button3's location
            point = new Point(button3.Location.X, Height - 68);

            //Set button3 location equal to point
            button3.Location = point;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            label1.Text = "Value: " + trackBar1.Value;
        }
    }
}