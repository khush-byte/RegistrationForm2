using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegistrationForm
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            label1.Text = Form3.info[0];
        }
    }
}
