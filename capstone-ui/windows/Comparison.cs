using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

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
            open.ShowDialog();

            //
            image1.path = open.FileName;

            //
            textBox2.Text = image1.path;
        }

        private void select2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                OpenFileDialog open = new OpenFileDialog();

                //
                open.ShowDialog();

                //
                image2.path = open.FileName;

                //
                textBox3.Text = image2.path;
            }
            else
            {
                Generate gen = new Generate(textBox2.Text);

                gen.ShowDialog();
            }
        }

        private void okay_Click(object sender, EventArgs e)
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

                if(Form1.comparisons[Index].image1 != image1 || Form1.comparisons[Index].image2 != image2)
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
    }
}
