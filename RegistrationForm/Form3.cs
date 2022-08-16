using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegistrationForm
{
    public partial class Form3 : Form
    {
        string connString = @"Data Source=(localdb)\MSSQLLocalDB; AttachDbFilename=|DataDirectory|\Database.mdf; Integrated Security=True;";
        string date;
        string commod = "";
        string unit_value = "";
        string district = "";
        string jamoat = "";
        string msgClean, msgFill, message, msgAdd, msgNumber, messageDel;
        public static String[] info;

        public Form3()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            date = DateTime.Now.ToString("dd.MM.yyyy");
            date_field_2.Text = date;
        }

        private void comboLocation_d_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboLocation_d.Text == "Дигар")
            {
                textLocation_d.Visible = true;
                textLocation2_d.Visible = true;
                comboLocation2_d.Enabled = false;
            }
            else
            {
                textLocation_d.Visible = false;
                textLocation2_d.Visible = false;
                comboLocation2_d.Enabled = true;
                string name = "";
                if (comboLocation_d.SelectedIndex == 0) name = "lakhsh";
                if (comboLocation_d.SelectedIndex == 1) name = "muminobod";
                selectLocation(name, comboLocation2_d);
            }

            district = comboLocation_d.SelectedItem.ToString();
        }

        private void selectLocation(string name, ComboBox item)
        {
            item.Items.Clear();
            string queryString = "SELECT jamoat FROM Location where district = '" + name + "';";
            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string str = reader.GetString(0);
                        item.Items.Add(str);
                    }
                }
                connection.Close();
                item.Items.Add("Дигар");
            }
        }

        private void btn_close_d_Click(object sender, EventArgs e)
        {
            Main form = new Main();
            this.Hide();
            form.ShowDialog();
            this.Close();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'databaseDataSet.Commodity' table. You can move, or remove it, as needed.
            this.commodityTableAdapter.Fill(this.databaseDataSet.Commodity);

            if (CultureInfo.CurrentCulture.Name.StartsWith("tg") == true)
            {
                messageDel = "Шумо маълумотро бо раками сатрии додашуда нест карданӣ ҳастед. Ин раванд бебозгашт аст. Оё шумо мехоҳед ин равандро идома диҳед? Агар рози бошед, 'Yes'-ро пахш кунед, вагарна 'No'-ро пахш кунед.";
                msgNumber = "Лутфан танҳо рақамҳоро ворид кунед!";
                msgAdd = "  Маълумоти нав бомуваффақият илова карда шуд!";
                msgFill = "  Лутфан ҳамаи майдонҳоро пур кунед!";
                msgClean = "Рӯйхати молҳо тоза карда мешавад, шумо розӣ ҳастед?";
                message = "Шумо маълумоти нав илова карда истодаед.\nАгар маълумоти воридшуда дуруст бошад, 'Ok'-ро пахш кунед, вагарна 'Cancel'-ро пахш кунед.";
            }
            else
            {
                messageDel = "You are about to delete the data with the given id. This process is irreversible. Do you want to continue the process?";
                msgNumber = "Please enter only numbers!";
                msgAdd = "  New data successfully added!";
                msgFill = "  Please fill in all fields!";
                msgClean = "The list of commodities will be cleaned, are you agree?";
                message = "You are about to add new data to the database. If the entered data is correct, click 'Yes', otherwise, click 'No' and edit the data.";
            }
        }

        private void comboCom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboCom.Text == "Дигар")
            {
                textCom.Visible = true;
                commod = textCom.Text.ToString();
            }
            else
            {
                textCom.Visible = false;
                commod = comboCom.SelectedItem.ToString();
            } 
        }

        private void comboUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboUnit.Text == "Дигар")
            {
                textUnit.Visible = true;
                unit_value = comboUnit.Text.ToString();
            }
            else
            {
                textUnit.Visible = false;
                unit_value = comboUnit.SelectedItem.ToString();
            }
        }

        private void totalField_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(totalField.Text, "[^0-9]"))
            {
                MessageBox.Show(msgNumber, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                totalField.Text = totalField.Text.Remove(totalField.Text.Length - 1);
            }
        }

        private void btn_next_d_Click(object sender, EventArgs e)
        {
            if (comboLocation_d.Text == "Дигар")
            {
                district = textLocation_d.Text.ToString();
                jamoat = textLocation2_d.Text.ToString();
            }
            if (comboLocation2_d.Text == "Дигар") jamoat = textLocation2_d.Text.ToString();

            if (comboProject_d.SelectedIndex != -1 && district.Length > 0 && jamoat.Length > 0 && textVil.Text.Length > 0 && textDist.Text.Length > 0 && textWitn.Text.Length > 0)
            {
                List<string> list = new List<string>();
                list.Add(comboProject_d.Text.ToString());
                list.Add(district);
                list.Add(jamoat);
                list.Add(textVil.Text.ToString());
                list.Add(textDist.Text.ToString());
                list.Add(textWitn.Text.ToString());
                info = list.ToArray();

                Form4 form = new Form4();
                this.Hide();
                form.ShowDialog();
                this.Close();
            }
            else 
            {
                MessageBox.Show(msgFill, "Massage", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void comboLocation2_d_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboLocation2_d.Text == "Дигар")
            {
                textLocation2_d.Visible = true;
            }
            else
            {
                textLocation2_d.Visible = false;
            }

            jamoat = comboLocation2_d.SelectedItem.ToString();
        }

        private void btn_edit_d_Click(object sender, EventArgs e)
        {
            int id = commodityDataGridView.CurrentCell.RowIndex;
            id += 1;

            var result = MessageBox.Show(messageDel, "Message",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                using (var conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string sql = "DELETE FROM Commodity WHERE Id = " + id + ";";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    InitializeDataGridView();
                }
            }
        }

        private void packFiled_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(packFiled.Text, "[^0-9]"))
            {
                MessageBox.Show(msgNumber, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                packFiled.Text = packFiled.Text.Remove(packFiled.Text.Length - 1);
            }
        }

        private void btn_new_d_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(msgClean, "Warning",
                                             MessageBoxButtons.OKCancel,
                                             MessageBoxIcon.Warning);

            // If the no button was pressed ...
            if (result == DialogResult.OK)
            {
                using (var conn = new SqlConnection(connString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Commodity", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    InitializeDataGridView();
                }
            }
        }

        private void InitializeDataGridView()
        {
            this.commodityDataGridView.DataSource = null;
            this.commodityDataGridView.Rows.Clear();

            using (SqlConnection con = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Commodity", con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            //sendTable = dt;
                            sda.Fill(dt);
                            commodityDataGridView.DataSource = dt;
                            if (commodityDataGridView.RowCount > 0)
                                commodityDataGridView.FirstDisplayedScrollingRowIndex = commodityDataGridView.RowCount - 1;
                        }
                    }
                    con.Close();
                }
            }
        }

        private void btn_add_d_Click(object sender, EventArgs e)
        {
            if (comboCom.Text == "Дигар") commod = textCom.Text.ToString();
            if (comboUnit.Text == "Дигар") unit_value = textUnit.Text.ToString();

            if (commod.Length > 0 && unit_value.Length > 0 && packFiled.Text.Length > 0 && totalField.Text.Length > 0 && conditionField.Text.Length > 0)
            {
                var result = MessageBox.Show(message, "Message",
                                             MessageBoxButtons.OKCancel,
                                             MessageBoxIcon.Warning);

                if (result == DialogResult.OK)
                {
                    using (var conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("commod_insert", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = commod;
                        cmd.Parameters.Add("@unit", SqlDbType.NVarChar).Value = unit_value;
                        cmd.Parameters.Add("@packageSum", SqlDbType.NVarChar).Value = packFiled.Text.ToString();
                        cmd.Parameters.Add("@total", SqlDbType.NVarChar).Value = totalField.Text.ToString();
                        cmd.Parameters.Add("@condition", SqlDbType.NVarChar).Value = conditionField.Text.ToString();

                        int i = cmd.ExecuteNonQuery();
                        if (i != 0)
                        {
                            MessageBox.Show(msgAdd, "Message", MessageBoxButtons.OK, MessageBoxIcon.None);
                            InitializeDataGridView();
                        }
                        conn.Close();                       
                    }

                    packFiled.Clear();
                    totalField.Clear();
                    conditionField.Clear();
                }
            }
            else MessageBox.Show(msgFill, "Massage", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
    }
}
