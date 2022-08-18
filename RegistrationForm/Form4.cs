using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegistrationForm
{
    public partial class Form4 : Form
    {
        List<CommodityData> list = new List<CommodityData>();
        string connString = @"Data Source=(localdb)\MSSQLLocalDB; AttachDbFilename=|DataDirectory|\Database.mdf; Integrated Security=True;";

        public Form4()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            string queryString = "SELECT name, unit, total FROM Commodity;";
            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    
                    while (reader.Read())
                    {
                        CommodityData comm = new CommodityData();
                        comm.name = reader.GetString(0);
                        comm.unit = reader.GetString(1);
                        comm.total = reader.GetInt32(2);
                        list.Add(comm);
                    }
                }
                connection.Close();

                for (int i = 0; list.Count > i; i++)
                {
                    int a = i + 1;
                    string b = list[i].name+" ("+ list[i].unit + ")";
                    listDataGridView.Columns.Add(a.ToString(), b);
                }
            }
        }
    }
}
