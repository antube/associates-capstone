using System;
using System.Drawing;
using System.Windows.Forms;

namespace capstone_ui.windows
{
    public partial class Generate : Form
    {
        public string bitmapPath = "";
        public string saveFilePath = "";
        public string quatizationMatrixPath = "";
        public int qualityIndex = 0;
        public int subSampling = 0;

        public Generate(string filePath)
        {
            InitializeComponent();

            bitmapPath = filePath;
        }

        private void Generate_Load(object sender, EventArgs e)
        {
            Width = 326;
            Height = 175;

            okButton.Location = new Point(142, Height - 66);

            cancelButton.Location = new Point(223, Height - 66);

            disableAllControls();
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
            Width = 326;
            Height = 210;

            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;

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
            //326, 358
            Width = 326;
            Height = 395;

            label1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;

            trackBar1.Visible = true;

            radioButton3.Visible = true;
            radioButton4.Visible = true;
            radioButton5.Visible = true;
            radioButton6.Visible = true;
            radioButton7.Visible = true;
            radioButton8.Visible = true;


        }

        private void enableSpecialMatrixControls()
        {
            //326, 358
            Width = 326;
            Height = 395;

            label3.Visible = true;
            label4.Visible = true;

            radioButton3.Visible = true;
            radioButton4.Visible = true;
            radioButton5.Visible = true;
            radioButton6.Visible = true;
            radioButton7.Visible = true;
            radioButton8.Visible = true;

            button4.Visible = true;

            textBox2.Visible = true;

            label4.Location = new Point(103, 117);

            button4.Location = new Point(9, 151);

            textBox2.Location = new Point(button4.Width + 10, 151);
        }

        private void Generate_SizeChanged(object sender, EventArgs e)
        {
            //Create New point to represent button2's location
            Point point = new Point(okButton.Location.X, Height - 68);

            //Set button2 location equal to point
            okButton.Location = point;

            //Create new point to represent button3's location
            point = new Point(cancelButton.Location.X, Height - 68);

            //Set button3 location equal to point
            cancelButton.Location = point;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            label1.Text = "Value: " + trackBar1.Value;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            saveFilePath = textBox1.Text;

            if (!isJpeg(saveFilePath))
            {
                MessageBox.Show("", "ERROR");
            }
            else
            {
                if (radioButton3.Checked)
                    subSampling = 0;
                else if (radioButton4.Checked)
                    subSampling = 1;
                else if (radioButton5.Checked)
                    subSampling = 2;
                else if (radioButton6.Checked)
                    subSampling = 3;
                else if (radioButton7.Checked)
                    subSampling = 4;
                else
                    subSampling = 5;

                if (radioButton1.Checked)
                    qualityIndex = trackBar1.Value;

                else
                    quatizationMatrixPath = textBox2.Text;

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void imageSaveLocation_Click(object sender, EventArgs e)
        {
            SaveFileDialog browser = new SaveFileDialog();

            if (browser.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = browser.FileName;
            }
        }

        private bool isJpeg(string name)
        {
            return (name.Substring(name.Length - 3).ToLower() == "jpg") ||
                (name.Substring(name.Length - 4).ToLower() == "jpeg");
        }
    }
}