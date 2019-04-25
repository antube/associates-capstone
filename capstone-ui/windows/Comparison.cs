using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace capstone_ui.windows
{
    public partial class Comparison : Form
    {
        public nodes.Image image1 = new nodes.Image();
        public nodes.Image image2 = new nodes.Image();

        public int Index = -1;

        public Comparison(int I)
        {
            InitializeComponent();

            Index = I;

            if (Index != -1)
            {
                textBox1.Text = Form1.comparisons[Index].name;
                textBox2.Text = Form1.comparisons[Index].image1.path;
                textBox3.Text = Form1.comparisons[Index].image2.path;
                textBox4.Text = Form1.comparisons[Index].image1.name;
                textBox5.Text = Form1.comparisons[Index].image2.name;

                image1.path = textBox2.Text;
                image2.path = textBox3.Text;

                //Create point and set equal to first coordinate set
                Point point = new Point(12, 81);

                //Set Select Button 2 equal to new coordinates
                select2.Location = point;

                //Set point equal to new coordinates
                point.X = 105;
                point.Y = 83;

                //Set Text Box 3 equal to new coordinates
                textBox3.Location = point;

                //Set point equal to new coordinates
                point.X = 367;
                point.Y = 89;

                //Set label 3 equal to new coordinates
                label3.Location = point;

                point.X = 402;
                point.Y = 82;

                textBox5.Location = point;

                point.X = 389;
                point.Y = 109;

                okay.Location = point;

                point.X = 470;
                point.Y = 109;

                cancel.Location = point;

                //Resize Form to proper size
                this.Width = 567;
                this.Height = 177;

                radioButton1.Visible = false;
                radioButton2.Visible = false;
            }
            else
            {

            }
        }

        private void select1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();

            //
            if (open.ShowDialog() == DialogResult.OK)
            {
                image1.path = open.FileName;
                if (properFormats(0))
                {
                    //
                    textBox2.Text = image1.path;
                }
                else
                {
                    image1.path = "";

                    MessageBox.Show("Non Bitmap Inputed", "ERROR");
                }
            }
        }

        private void select2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                OpenFileDialog open = new OpenFileDialog();

                //
                if (open.ShowDialog() == DialogResult.OK)
                {
                    image2.path = open.FileName;

                    if (properFormats(1))
                    {
                        //
                        textBox3.Text = image2.path;
                    }
                    else
                    {
                        image2.path = "";
                        MessageBox.Show("Non Jpeg file Format", "ERROR");
                    }
                }
            }
            else
            {
                Generate gen = new Generate(textBox2.Text);

                if (gen.ShowDialog() == DialogResult.OK)
                {
                    image2.path = gen.filePath;

                    textBox3.Text = gen.filePath;

                    Process process = new Process();

                    process.StartInfo.FileName = "capstoneCompression.exe";

                    process.StartInfo.Arguments = "";
                }
            }
        }

        private void okay_Click(object sender, EventArgs e)
        {

            if (!filesValidInput())
            {
                MessageBox.Show("Comparison Name Empty, Files Do Not Exist, or No Files Specified", "ERROR");
            }
            else
            {

                image1.name = textBox4.Text;

                image2.name = textBox5.Text;

                if (Index == -1)
                {
                    Form1.comparisons.Add(new nodes.Comparison { name = textBox1.Text, image1 = image1, image2 = image2 });
                }
                else
                {
                    bool filesDeleted = false;

                    if (Form1.comparisons[Index].image1 != image1 || Form1.comparisons[Index].image2 != image2)
                    {
                        File.Delete(Form1.comparisons[Index].name + ".jpg.csv");
                        File.Delete(Form1.comparisons[Index].name + ".bmp.csv");
                        File.Delete(Form1.comparisons[Index].name + ".bmp");
                        filesDeleted = true;
                    }

                    Form1.comparisons[Index].name = textBox1.Text;
                    Form1.comparisons[Index].image1 = image1;
                    Form1.comparisons[Index].image2 = image2;

                    if (filesDeleted)
                        Form1.generate_data((int)Index);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            select2.Text = "Select Image 2";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            select2.Text = "Generate";
        }

        private bool properFormats(int i = 1)
        {
            if (i == 0)
                return image1.path[image1.path.Length - 3] == 'b' && image1.path[image1.path.Length - 2] == 'm' && image1.path[image1.path.Length - 1] == 'p';
            else
                return (image2.path[image2.path.Length - 4] == 'j' && image2.path[image2.path.Length - 3] == 'p' && image2.path[image2.path.Length - 2] == 'e' && image2.path[image2.path.Length - 1] == 'g') ||
                (image2.path[image2.path.Length - 3] == 'j' && image2.path[image2.path.Length - 2] == 'p' && image2.path[image2.path.Length - 1] == 'g');
        }

        public bool filesValidInput()
        {
            return (image1.path != "" && File.Exists(image1.path) &&
                //Is image path not null                   Does file exist
                image2.path != "" && File.Exists(image2.path) &&
                //Is name not equal to null
                textBox1.Text != "");
        }
    }
}