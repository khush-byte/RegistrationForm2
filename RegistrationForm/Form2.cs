using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegistrationForm
{
    public partial class Form2 : Form
    {
        private PrintDocument printDocument1 = new PrintDocument();

        public Form2()
        {
            InitializeComponent();
            GoFullscreen(true);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            PaperSize ps = new PaperSize();
            ps.RawKind = (int)PaperKind.A4;
            printDocument1.DefaultPageSettings.PaperSize = ps;
            printDocument1.DefaultPageSettings.Landscape = true;
        }

        Bitmap memoryImage;

        private void CaptureScreen()
        {
            Graphics myGraphics = this.CreateGraphics();
            Size s = this.Size;
            memoryImage = new Bitmap(s.Width, s.Height, myGraphics);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CopyFromScreen(this.Location.X, this.Location.Y, 0, 0, s);
        }

        private void printDocument1_PrintPage(System.Object sender,
               System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(memoryImage, 0, 0);
        }

        private void GoFullscreen(bool fullscreen)
        {
            if (fullscreen)
            {
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.Bounds = Screen.PrimaryScreen.Bounds;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            }
        }

        public async void WaitSomeTime()
        {
            PrintDialog printdlg = new PrintDialog();
           
            if (trainingDataGridView.RowCount < 23)
            {
                await Task.Delay(3000);
                CaptureScreen();
                printDocument1.DefaultPageSettings.Landscape = true;
                printdlg.Document = printDocument1;
                if (printdlg.ShowDialog() == DialogResult.OK) printDocument1.Print();
                this.Close();
            }
            else if (trainingDataGridView.RowCount > 22 && trainingDataGridView.RowCount < 45) 
            {
                await Task.Delay(2000);
                CaptureScreen();
                printDocument1.DefaultPageSettings.Landscape = true;
                printdlg.Document = printDocument1;
                if (printdlg.ShowDialog() == DialogResult.OK) printDocument1.Print();

                deletePage(1);

                await Task.Delay(2000);
                CaptureScreen();
                printDocument1.DefaultPageSettings.Landscape = true;
                printdlg.Document = printDocument1;
                if (printdlg.ShowDialog() == DialogResult.OK) printDocument1.Print();

                this.Close();
            }
            else if (trainingDataGridView.RowCount > 40 && trainingDataGridView.RowCount < 61)
            {
                await Task.Delay(2000);
                CaptureScreen();
                printDocument1.DefaultPageSettings.Landscape = true;
                printdlg.Document = printDocument1;
                if (printdlg.ShowDialog() == DialogResult.OK) printDocument1.Print();

                deletePage(1);

                await Task.Delay(2000);
                CaptureScreen();
                printDocument1.DefaultPageSettings.Landscape = true;
                printdlg.Document = printDocument1;
                if (printdlg.ShowDialog() == DialogResult.OK) printDocument1.Print();


                deletePage(2);

                await Task.Delay(2000);
                CaptureScreen();
                printDocument1.DefaultPageSettings.Landscape = true;
                printdlg.Document = printDocument1;
                if (printdlg.ShowDialog() == DialogResult.OK) printDocument1.Print();

                this.Close();
            }
            else if (trainingDataGridView.RowCount > 60 && trainingDataGridView.RowCount < 81)
            {
                /*PaperSize ps = new PaperSize();
                ps.RawKind = (int)PaperKind.A4;
                printDocument1.DefaultPageSettings.PaperSize = ps;*/

                await Task.Delay(2000);
                CaptureScreen();
                printDocument1.DefaultPageSettings.Landscape = true;
                printdlg.Document = printDocument1;
                if (printdlg.ShowDialog() == DialogResult.OK) printDocument1.Print();

                deletePage(1);

                await Task.Delay(2000);
                CaptureScreen();
                printDocument1.DefaultPageSettings.Landscape = true;
                printdlg.Document = printDocument1;
                if (printdlg.ShowDialog() == DialogResult.OK) printDocument1.Print();

                deletePage(2);

                await Task.Delay(2000);
                CaptureScreen();
                printDocument1.DefaultPageSettings.Landscape = true;
                printdlg.Document = printDocument1;
                if (printdlg.ShowDialog() == DialogResult.OK) printDocument1.Print();

                deletePage(3);

                await Task.Delay(2000);
                CaptureScreen();
                printDocument1.DefaultPageSettings.Landscape = true;
                printdlg.Document = printDocument1;
                if (printdlg.ShowDialog() == DialogResult.OK) printDocument1.Print();

                this.Close();
            }
            else if (trainingDataGridView.RowCount > 80 && trainingDataGridView.RowCount < 101)
            {
                await Task.Delay(2000);
                CaptureScreen();
                printDocument1.DefaultPageSettings.Landscape = true;
                printdlg.Document = printDocument1;
                if (printdlg.ShowDialog() == DialogResult.OK) printDocument1.Print();

                deletePage(1);

                await Task.Delay(2000);
                CaptureScreen();
                printDocument1.DefaultPageSettings.Landscape = true;
                printdlg.Document = printDocument1;
                if (printdlg.ShowDialog() == DialogResult.OK) printDocument1.Print();

                deletePage(2);

                await Task.Delay(2000);
                CaptureScreen();
                printDocument1.DefaultPageSettings.Landscape = true;
                printdlg.Document = printDocument1;
                if (printdlg.ShowDialog() == DialogResult.OK) printDocument1.Print();

                deletePage(3);

                await Task.Delay(2000);
                CaptureScreen();
                printDocument1.DefaultPageSettings.Landscape = true;
                printdlg.Document = printDocument1;
                if (printdlg.ShowDialog() == DialogResult.OK) printDocument1.Print();

                deletePage(4);

                await Task.Delay(2000);
                CaptureScreen();
                printDocument1.DefaultPageSettings.Landscape = true;
                printdlg.Document = printDocument1;
                if (printdlg.ShowDialog() == DialogResult.OK) printDocument1.Print();

                this.Close();
            }
            else if (trainingDataGridView.RowCount > 100)
            {
                await Task.Delay(2000);
                CaptureScreen();
                printDocument1.DefaultPageSettings.Landscape = true;
                printdlg.Document = printDocument1;
                if (printdlg.ShowDialog() == DialogResult.OK) printDocument1.Print();

                deletePage(1);

                await Task.Delay(2000);
                CaptureScreen();
                printDocument1.DefaultPageSettings.Landscape = true;
                printdlg.Document = printDocument1;
                if (printdlg.ShowDialog() == DialogResult.OK) printDocument1.Print();

                deletePage(2);

                await Task.Delay(2000);
                CaptureScreen();
                printDocument1.DefaultPageSettings.Landscape = true;
                printdlg.Document = printDocument1;
                if (printdlg.ShowDialog() == DialogResult.OK) printDocument1.Print();

                deletePage(3);

                await Task.Delay(2000);
                CaptureScreen();
                printDocument1.DefaultPageSettings.Landscape = true;
                printdlg.Document = printDocument1;
                if (printdlg.ShowDialog() == DialogResult.OK) printDocument1.Print();

                deletePage(4);

                await Task.Delay(2000);
                CaptureScreen();
                printDocument1.DefaultPageSettings.Landscape = true;
                printdlg.Document = printDocument1;
                if (printdlg.ShowDialog() == DialogResult.OK) printDocument1.Print();

                deletePage(5);

                await Task.Delay(2000);
                CaptureScreen();
                printDocument1.DefaultPageSettings.Landscape = true;
                printdlg.Document = printDocument1;
                if (printdlg.ShowDialog() == DialogResult.OK) printDocument1.Print();

                this.Close();
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'databaseDataSet.Training' table. You can move, or remove it, as needed.
            //this.trainingTableAdapter.Fill(this.databaseDataSet.Training);
            //SecondPage();
            WaitSomeTime();
        }

        public void SetValues(string date, string project, string location, string category, string topic, string trainers)
        {
            labelDate.Text = "Date of training / Сана: " + date;
            labelProject.Text = project;
            labelLocation.Text = location;
            labelCategory.Text = category;
            labelTopic.Text = topic;
            labelTrainers.Text = trainers;
            InitializeDataGridView(date);
        }

        private void trainingBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.trainingBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.databaseDataSet);
        }

        public void deletePage(int num){
            CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[trainingDataGridView.DataSource];

            if (num == 1)
            {
                for (int i = 0; i < 22; i++)
                {
                    currencyManager1.SuspendBinding();
                    trainingDataGridView.Rows[i].Visible = false;
                    currencyManager1.ResumeBinding();
                }
            }
            else if (num == 2)
            {
                for (int i = 0; i < 44; i++)
                {
                    currencyManager1.SuspendBinding();
                    trainingDataGridView.Rows[i].Visible = false;
                    currencyManager1.ResumeBinding();
                }
            }
            else if (num == 3)
            {
                for (int i = 0; i < 66; i++)
                {
                    currencyManager1.SuspendBinding();
                    trainingDataGridView.Rows[i].Visible = false;
                    currencyManager1.ResumeBinding();
                }
            }
            else if (num == 4)
            {
                for (int i = 0; i < 88; i++)
                {
                    currencyManager1.SuspendBinding();
                    trainingDataGridView.Rows[i].Visible = false;
                    currencyManager1.ResumeBinding();
                }
            }
            else if (num == 5)
            {
                for (int i = 0; i < 110; i++)
                {
                    currencyManager1.SuspendBinding();
                    trainingDataGridView.Rows[i].Visible = false;
                    currencyManager1.ResumeBinding();
                }
                //this.tableLayoutPanel2.RowStyles[8].Height = 0;
            }
        }

        private void InitializeDataGridView(string date)
        {
            this.trainingDataGridView.DataSource = null;
            this.trainingDataGridView.Rows.Clear();
            string connString = @"Data Source=(localdb)\MSSQLLocalDB; AttachDbFilename=|DataDirectory|\Database.mdf; Integrated Security=True;";

            using (SqlConnection con = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Training WHERE Date = '" + date + "';", con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {                            
                            sda.Fill(dt);
                            trainingDataGridView.DataSource = dt;
                        }
                    }
                    con.Close();
                }
            }
        }
    }
}
