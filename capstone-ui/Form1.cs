using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace capstone_ui
{
    public partial class Form1 : Form
    {
        //List of object Comparison
        public static List<nodes.Comparison> comparisons = new List<nodes.Comparison>() { };


        //
        public Form1()
        {
            InitializeComponent();
        }


        //
        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox2.Enabled = false;
            pictureBox2.Visible = false;

            pictureBox1.Dock = DockStyle.Fill;

            chart1.Width = tabPage2.Width;
            chart1.Height = (tabPage2.Height >> 1) - (menuStrip4.Height >> 1);

            chart2.Width = tabPage2.Width;
            chart2.Height = (tabPage2.Height >> 1) - (menuStrip4.Height >> 1);

            Point c1 = new Point();
            c1.X = 0;
            c1.Y = 0;

            chart1.Location = c1;

            Point c2 = new Point();
            c2.X = 0;
            c2.Y = (tabPage2.Height >> 1) - (menuStrip4.Height >> 1);

            chart2.Location = c2;
        }


        //Panel 1
        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            windows.Comparison comparison = new windows.Comparison(-1);

            if (comparison.ShowDialog() == DialogResult.OK)
            {
                treeView1_Update();

                int index = comparisons.Count - 1;

                generate_data(index);

                pictureBox3.ImageLocation = comparisons[index].name + ".bmp";
            }
        }


        //
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (comparisons.Count == 0)
                MessageBox.Show("EDIT ERROR: No Comparisons to Edit.");

            else if (treeView1.SelectedNode == null)
                MessageBox.Show("EDIT ERROR: No Comparison Selected to Edit.");

            else
            {
                int index = 0;

                if (treeView1.SelectedNode.Level == 0)
                    index = treeView1.SelectedNode.Index;
                else
                    index = treeView1.SelectedNode.Parent.Index;

                windows.Comparison comparison = new windows.Comparison(index);
                comparison.Text = "Edit Comparison";

                if (comparison.ShowDialog() == DialogResult.OK)
                    treeView1_Update();
            }
        }

        //
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (comparisons.Count == 0)
                MessageBox.Show("DELETE ERROR: No Comparisons to Delete.");

            else if (treeView1.SelectedNode == null)
                MessageBox.Show("DELETE ERROR: No Comparison Selected to Delete.");

            else
            {
                int index = 0;

                if (treeView1.SelectedNode.Level == 0)
                    index = treeView1.SelectedNode.Index;
                else
                    index = treeView1.SelectedNode.Parent.Index;

                deleteFiles(index);
                comparisons.RemoveAt(index);
                treeView1.Nodes.RemoveAt(index);
            }

            pictureBox1.ImageLocation = "";
            pictureBox2.ImageLocation = "";
        }


        //Panel 2
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            chart1.Width = tabPage3.Width;
            chart1.Height = (tabPage3.Height >> 1) - (menuStrip4.Height >> 1);

            chart2.Width = tabPage3.Width;
            chart2.Height = (tabPage3.Height >> 1) - (menuStrip4.Height >> 1);

            Point c1 = new Point();
            c1.X = 0;
            c1.Y = 0;

            chart1.Location = c1;

            Point c2 = new Point();
            c2.X = 0;
            c2.Y = (tabPage3.Height >> 1) - (menuStrip4.Height >> 1);

            chart2.Location = c2;
        }


        //
        public void treeView1_Update()
        {
            //Clear Treeview
            treeView1.Nodes.Clear();

            //Loop through comparisons list
            for (int i = 0; i < comparisons.Count; i++)
            {
                //
                TreeNode[] nodes = new TreeNode[2];

                //
                nodes[0] = new TreeNode(comparisons[i].image1.name);

                //
                nodes[1] = new TreeNode(comparisons[i].image2.name);

                //
                TreeNode node = new TreeNode(comparisons[i].name, nodes);

                //
                treeView1.Nodes.Add(node);
            }
        }


        //
        public static void generate_data(int index)
        {
            //Create Instance of Process Object
            Process process = new Process();

            //Specify Program to run
            process.StartInfo.FileName = "capstone_test.exe";

            //Hide window when processing data
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            //Execute Program as a Shell Program
            process.StartInfo.UseShellExecute = true;

            //Set Arguments for process
            process.StartInfo.Arguments = " " +
                
                //Input Jpeg Image Path
                "\"" + comparisons[index].image2.path + "\" " +
                
                //Input Bitmap Image Path
                "\"" + comparisons[index].image1.path + "\" " +
                
                //Output jpeg CSV Path
                "\"" + comparisons[index].name + ".jpg.csv\" " +
                
                //Output bitmap CSV Path
                "\"" + comparisons[index].name + ".bmp.csv\" " +
                
                //Output Difference Map Bitmap Path
                "\"" + comparisons[index].name + ".bmp\"";

            //Start Process
            process.Start();

            //Wait until process has exited
            process.WaitForExit();

            //If process exit code is one
            if (process.ExitCode == 1)
                //Alert user to Data Generation Failer
                MessageBox.Show("Comparison Data Generation Failed", "ERROR");
        }


        //
        public void deleteFiles(int index)
        {
            File.Delete(comparisons[index].name + ".jpg.csv");
            File.Delete(comparisons[index].name + ".bmp.csv");
            File.Delete(comparisons[index].name + ".bmp");
        }


        ////////////////////////////////////
        // updateCharts
        //
        // Type:
        //      
        ////////////////////////////////////
        public void updateCharts(int index)
        {
            FileInfo jpgInfo = new FileInfo(comparisons[index].name + ".jpg.csv");
            FileInfo bmpInfo = new FileInfo(comparisons[index].name + ".bmp.csv");
            StreamReader jpgCSV = new StreamReader(comparisons[index].name + ".jpg.csv");
            StreamReader bmpCSV = new StreamReader(comparisons[index].name + ".bmp.csv");

            char[] jpgCharBuffer = new char[jpgInfo.Length];
            char[] bmpCharBuffer = new char[bmpInfo.Length];

            jpgCSV.Read(jpgCharBuffer, 0, (int)jpgInfo.Length);
            bmpCSV.Read(bmpCharBuffer, 0, (int)bmpInfo.Length);

            jpgCSV.Close();
            bmpCSV.Close();

            int[] jpgIntBuffer = new int[1024];
            int[] bmpIntBuffer = new int[1024];

            string value = "";
            index = 0;

            for (int i = 0; i < jpgCharBuffer.Length; i++)
            {
                if (jpgCharBuffer[i] == ',' || jpgCharBuffer.Length == index)
                {
                    int val = Convert.ToInt32(value);
                    jpgIntBuffer[index] = val;
                    index++;
                    value = "";
                }
                else
                    value += jpgCharBuffer[i];
            }

            index = 0;

            for (int i = 0; i < bmpCharBuffer.Length; i++)
            {
                if (bmpCharBuffer[i] == ',' || bmpCharBuffer.Length == index)
                {
                    int val = Convert.ToInt32(value);
                    bmpIntBuffer[index] = val;
                    index++;
                    value = "";
                }
                else
                    value += bmpCharBuffer[i];
            }

            chart1.Series.Clear();
            chart2.Series.Clear();

            index = 0;

            Series jpgSeries = new Series("Red");
            Series bmpSeries = new Series("Red");

            jpgSeries.Enabled = true;
            bmpSeries.Enabled = true;

            jpgSeries.ChartArea = chart1.ChartAreas[0].Name;
            bmpSeries.ChartArea = chart2.ChartAreas[0].Name;

            jpgSeries.ChartType = SeriesChartType.Line;
            bmpSeries.ChartType = SeriesChartType.Line;

            jpgSeries.Color = Color.Red;
            bmpSeries.Color = Color.Red;

            while (index < 1024)
            {
                switch (index)
                {
                    case 255:
                        chart1.Series.Add(jpgSeries);
                        chart2.Series.Add(bmpSeries);

                        jpgSeries = new Series("Green");
                        bmpSeries = new Series("Green");

                        jpgSeries.Enabled = true;
                        bmpSeries.Enabled = true;

                        jpgSeries.Color = Color.Green;
                        bmpSeries.Color = Color.Green;

                        jpgSeries.ChartArea = chart1.ChartAreas[0].Name;
                        bmpSeries.ChartArea = chart2.ChartAreas[0].Name;

                        jpgSeries.ChartType = SeriesChartType.Line;
                        bmpSeries.ChartType = SeriesChartType.Line;
                        break;

                    case 511:
                        chart1.Series.Add(jpgSeries);
                        chart2.Series.Add(bmpSeries);

                        jpgSeries = new Series("Blue");
                        bmpSeries = new Series("Blue");

                        jpgSeries.Enabled = true;
                        bmpSeries.Enabled = true;

                        jpgSeries.Color = Color.Blue;
                        bmpSeries.Color = Color.Blue;

                        jpgSeries.ChartArea = chart1.ChartAreas[0].Name;
                        bmpSeries.ChartArea = chart2.ChartAreas[0].Name;

                        jpgSeries.ChartType = SeriesChartType.Line;
                        bmpSeries.ChartType = SeriesChartType.Line;
                        break;

                    case 767:
                        chart1.Series.Add(jpgSeries);
                        chart2.Series.Add(bmpSeries);

                        jpgSeries = new Series("Average");
                        bmpSeries = new Series("Average");

                        jpgSeries.Enabled = true;
                        bmpSeries.Enabled = true;

                        jpgSeries.Color = Color.DarkGray;
                        bmpSeries.Color = Color.DarkGray;

                        jpgSeries.ChartArea = chart1.ChartAreas[0].Name;
                        bmpSeries.ChartArea = chart2.ChartAreas[0].Name;

                        jpgSeries.ChartType = SeriesChartType.Line;
                        bmpSeries.ChartType = SeriesChartType.Line;
                        break;
                }

                jpgSeries.Points.AddY(jpgIntBuffer[index]);

                bmpSeries.Points.AddY(bmpIntBuffer[index]);

                index++;
            }
            chart1.Series.Add(jpgSeries);
            chart2.Series.Add(bmpSeries);

            if (chart2.ChartAreas[0].AxisY.Maximum > chart1.ChartAreas[0].AxisY.Maximum)
            {
                chart2.ChartAreas[0].AxisY.Maximum = chart1.ChartAreas[0].AxisY.Maximum;
                
            }
            else if (chart2.ChartAreas[0].AxisY.Maximum < chart1.ChartAreas[0].AxisY.Maximum)
            {
                chart1.ChartAreas[0].AxisY.Maximum = chart2.ChartAreas[0].AxisY.Maximum;
            }

        }


        ////////////////////////////////////
        // treeView1_AfterSelect
        //
        // Type:
        //      Event Handler
        //
        // Arguments:
        //           sender : object
        //           e      : TreeViewEventArgs
        //
        // Return:
        //        void
        ////////////////////////////////////
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Level == 0)
                {
                    pictureBox1.Dock = DockStyle.None;

                    Point point = new Point(0, 0);
                    pictureBox1.Location = point;

                    pictureBox1.Height = tabPage1.Height / 2;
                    pictureBox1.Width = tabPage1.Width;

                    pictureBox2.Enabled = true;
                    pictureBox2.Visible = true;

                    pictureBox2.Height = tabPage1.Height / 2;
                    pictureBox2.Width = tabPage1.Width;

                    point = new Point(0, tabPage1.Height / 2);

                    pictureBox2.Location = point;

                    pictureBox1.ImageLocation = comparisons[treeView1.SelectedNode.Index].image1.path;
                    pictureBox2.ImageLocation = comparisons[treeView1.SelectedNode.Index].image2.path;
                    pictureBox3.ImageLocation = comparisons[treeView1.SelectedNode.Index].name + ".bmp";
                    updateCharts(treeView1.SelectedNode.Index);
                }
                else
                {
                    pictureBox2.Enabled = false;
                    pictureBox2.Visible = false;

                    pictureBox1.Dock = DockStyle.Fill;


                    switch (treeView1.SelectedNode.Index)
                    {
                        case 0:
                            pictureBox1.ImageLocation = comparisons[treeView1.SelectedNode.Parent.Index].image1.path;
                            break;
                        case 1:
                            pictureBox1.ImageLocation = comparisons[treeView1.SelectedNode.Parent.Index].image2.path;
                            break;
                    }

                    pictureBox3.ImageLocation = comparisons[treeView1.SelectedNode.Parent.Index].name + ".bmp";
                    updateCharts(treeView1.SelectedNode.Parent.Index);
                }


            }
        }


        ///////////////////////////////////
        // tabPage3_Click
        //
        // Type:
        //      Event Handler
        //
        // Arguments:
        //           sender : object
        //           e      : EventArgs
        //
        // Return:
        //       void
        ///////////////////////////////////
        private void tabPage3_Click(object sender, EventArgs e)
        {
            int index = -1;

            if (treeView1.SelectedNode != null)
            {

                if (treeView1.SelectedNode.Level == 0)
                    index = treeView1.SelectedNode.Index;
                else
                    index = treeView1.SelectedNode.Parent.Index;
            }

            if (index != -1)
                pictureBox3.Name = comparisons[index].name + ".bmp";
        }


        ///////////////////////////
        // Form1_FormClosed
        // 
        // Type:
        //      Event Handler
        //
        // Arguments:
        //           sender : object
        //           e      : FormClosedEventArgs
        //
        // Return:
        //        void
        ///////////////////////////
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }


        ///////////////////////////////
        // tabControl1_SelectedIndexChanged
        // Type:
        //      Event Handler
        //
        // Arguments:
        //           sender : object
        //           e      : EventArgs
        //
        // Return:
        //        void
        ////////////////////////////////
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        

        ///////////////////////////
        // reToolStripMenuItem_CheckedChanged
        // Type:
        //      Event Handler
        //
        // Arguments:
        //           sender : object
        //           e      : EventArgs
        //
        // Returns:
        //         void
        ///////////////////////
        private void redToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (redToolStripMenuItem.Checked)
            {
                chart1.Series[0].Enabled = true;
                chart2.Series[0].Enabled = true;
            }
            else
            {
                chart1.Series[0].Enabled = false;
                chart2.Series[0].Enabled = false;
            }
        }


        //
        private void greenToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (greenToolStripMenuItem.Checked)
            {
                chart1.Series[1].Enabled = true;
                chart2.Series[1].Enabled = true;
            }
            else
            {
                chart1.Series[1].Enabled = false;
                chart2.Series[1].Enabled = false;
            }
        }


        //
        private void blueToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (blueToolStripMenuItem.Checked)
            {
                chart1.Series[2].Enabled = true;
                chart2.Series[2].Enabled = true;
            }
            else
            {
                chart1.Series[2].Enabled = false;
                chart2.Series[2].Enabled = false;
            }
        }


        //
        private void averageToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (averageToolStripMenuItem.Checked)
            {
                chart1.Series[3].Enabled = true;
                chart2.Series[3].Enabled = true;
            }
            else
            {
                chart1.Series[3].Enabled = false;
                chart2.Series[3].Enabled = false;
            }
        }


        //
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            for(int i = 0; i < comparisons.Count; i++)
            {
                deleteFiles(i);
            }
        }


        //
        private void splitContainer1_Panel2_SizeChanged(object sender, EventArgs e)
        {
            tabControl1.Width = splitContainer1.Panel2.Width;
            tabControl1.Height = splitContainer1.Panel2.Height;
        }


        //
        private void tabPage2_SizeChanged(object sender, EventArgs e)
        {
            //Set Chart One Width to page width
            chart1.Width = tabPage2.Width;

            //Set chart one height to half page height minus half bottom menustrip height
            chart1.Height = (tabPage2.Height >> 1) - (menuStrip4.Height >> 1);

            //Set Chart 2 width  to page width
            chart2.Width = tabPage2.Width;

            //Set Chart height to half page minus half menustrip4 height
            chart2.Height = (tabPage2.Height >> 1) - (menuStrip4.Height >> 1);

            //Create new instance of point object
            Point c1 = new Point();

            //Assign 0 to 
            c1.X = 0;
            c1.Y = 0;

            chart1.Location = c1;

            Point c2 = new Point();
            c2.X = 0;
            c2.Y = (tabPage2.Height >> 1) - (menuStrip4.Height >> 1);

            chart2.Location = c2;
        }
    }
}