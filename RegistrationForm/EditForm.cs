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
    public partial class EditForm : Form
    {
        int id = 0;
        string connString = @"Data Source=(localdb)\MSSQLLocalDB; AttachDbFilename=|DataDirectory|\Database.mdf; Integrated Security=True;";
        string type = "";
        int lunchCheck = 0;
        int stationaryCheck = 0;
        int coffeeCheck = 0;
        string gender = "";
        decimal accommodation = 0;
        decimal transportationCost = 0;
        int age = -1;
        string other = "";
        string districtName = "";
        string jamoatName = "";
        string msgNumber;
        string message;
        string caption;
        string messageDel, msgUpdated;
        string captionDel, msgNotExist, msgDeleted;

        public EditForm()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void applyBtn_Click(object sender, EventArgs e)
        {
            if (idField.Text.Length > 0)
            {
                if (e_comboType.Text == "Дигар") type = e_textOther.Text;

                string first_name = e_textParName.Text.ToString().Replace(" ", "");
                string last_name = e_textParName2.Text.ToString().Replace(" ", "");
                id = Int32.Parse(idField.Text);

                if (e_textOther.Visible)
                    other = e_textOther.Text;
                else other = "";

                string accom = e_textAccommodation.Text.ToString();
                accom = accom.Replace(',', '.');
                if (accom.Length > 0)
                {
                    if (!accom.Equals("."))
                        accommodation = Decimal.Parse(accom, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
                    else accommodation = 0;
                }
                else accommodation = 0;

                string trans = e_textTransportCost.Text.ToString();
                trans = trans.Replace(',', '.');
                if (trans.Length > 0)
                {
                    if (!trans.Equals("."))
                        transportationCost = Decimal.Parse(trans, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
                    else transportationCost = 0;
                }
                else transportationCost = 0;

                if (e_textAge.Text.Length > 0)
                    age = Int32.Parse(e_textAge.Text);
                else age = -1;

                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.OKCancel,
                                             MessageBoxIcon.Warning);

                if (result == DialogResult.OK)
                {
                    using (var conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("sp_edit", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                        cmd.Parameters.Add("@qr", SqlDbType.VarChar).Value = e_textQR.Text.ToString();
                        cmd.Parameters.Add("@parNameFirst", SqlDbType.NVarChar).Value = first_name;
                        cmd.Parameters.Add("@parNameLast", SqlDbType.NVarChar).Value = last_name;
                        cmd.Parameters.Add("@gender", SqlDbType.NVarChar).Value = gender;
                        cmd.Parameters.Add("@passport", SqlDbType.NVarChar).Value = e_textPassport.Text.ToString();
                        cmd.Parameters.Add("@age", SqlDbType.VarChar).Value = age;
                        cmd.Parameters.Add("@type", SqlDbType.NVarChar).Value = type;
                        cmd.Parameters.Add("@other", SqlDbType.VarChar).Value = "";
                        cmd.Parameters.Add("@district", SqlDbType.NVarChar).Value = districtName;
                        cmd.Parameters.Add("@jamoat", SqlDbType.NVarChar).Value = jamoatName;
                        cmd.Parameters.Add("@village", SqlDbType.NVarChar).Value = e_textVillage.Text.ToString();
                        cmd.Parameters.Add("@phone", SqlDbType.NVarChar).Value = e_textPhone.Text.ToString();
                        cmd.Parameters.Add("@coffee", SqlDbType.VarChar).Value = coffeeCheck;
                        cmd.Parameters.Add("@lunch", SqlDbType.VarChar).Value = lunchCheck;
                        cmd.Parameters.Add("@stationary", SqlDbType.VarChar).Value = stationaryCheck;
                        cmd.Parameters.Add("@transport", SqlDbType.Decimal).Value = transportationCost;
                        cmd.Parameters.Add("@accom", SqlDbType.Decimal).Value = accommodation;

                        int i = cmd.ExecuteNonQuery();
                        if (i != 0)
                        {
                            MessageBox.Show(msgUpdated, "Хабар", MessageBoxButtons.OK, MessageBoxIcon.None);
                        }
                        conn.Close();
                    }
                }
            }
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            if (idField.Text.Length > 0)
            {
                var result = MessageBox.Show(messageDel, captionDel,
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Warning);

                // If the no button was pressed ...
                if (result == DialogResult.Yes)
                {
                    id = Int32.Parse(idField.Text);
                    using (var conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("sp_delete", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                        int i = cmd.ExecuteNonQuery();
                        if (i != 0)
                        {                
                            MessageBox.Show(msgDeleted, "Хабар", MessageBoxButtons.OK, MessageBoxIcon.None);
                        }
                        else 
                        {
                            MessageBox.Show(msgNotExist, "Хабар", MessageBoxButtons.OK, MessageBoxIcon.None);
                        }
                        conn.Close();
                    }
                }
            }
        }

        private void buttonPrint_Click(object sender, EventArgs e)
        {
            e_textQR.Clear();
            e_textParName.Clear();
            e_textPassport.Clear();
            e_textAge.Clear();
            e_comboGender.SelectedIndex = -1;
            e_comboType.SelectedIndex = -1;
            e_comboDistrict.SelectedIndex = -1;
            e_comboJamoat.Items.Clear();
            e_textVillage.Clear();
            e_textPhone.Clear();
            e_checkCoffee.Checked = false;
            e_checkLunch.Checked = false;
            e_checkStationary.Checked = false;
            e_textTransportCost.Clear();
            e_textAccommodation.Clear();
            e_textParName2.Clear();

            if (idField.Text.Length > 0) 
            {
                id = Int32.Parse(idField.Text);

                string queryString = "SELECT QR, ParticipantName, SecondName, Gender, PassportID, Age, ParticipantType, Address, AddressJumoat, AddressVillage, Phone, CoffeeBreak, Lunch, Stationary, TransportCost, Accommodation FROM Training where id = " + id;
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string str = reader.GetString(0);
                            if (!reader.IsDBNull(0)) e_textQR.Text = reader.GetString(0);
                            if (!reader.IsDBNull(1)) e_textParName.Text = reader.GetString(1);
                            if (!reader.IsDBNull(2)) e_textParName2.Text = reader.GetString(2);

                            if (!reader.IsDBNull(3))
                            {
                                if (reader.GetString(3).Equals("Мард")) e_comboGender.SelectedIndex = 0;
                                else if (reader.GetString(3).Equals("Зан")) e_comboGender.SelectedIndex = 1;
                            }

                            if (!reader.IsDBNull(4)) e_textPassport.Text = reader.GetString(4);
                            if (!reader.IsDBNull(5)) e_textAge.Text = reader.GetInt32(5).ToString();

                            if (!reader.IsDBNull(6))
                            {
                                if (reader.GetString(6).Equals("Деҳқон")) e_comboType.SelectedIndex = 0;
                                else if (reader.GetString(6).Equals("Агроном")) e_comboType.SelectedIndex = 1;
                                else if (!reader.GetString(6).Equals(""))
                                {
                                    e_comboType.SelectedIndex = 2;
                                    if (!reader.IsDBNull(6)) e_textOther.Text = reader.GetString(6);
                                }
                            }

                            if (!reader.IsDBNull(7))
                            {
                                if (reader.GetString(7).Equals("Лахш"))
                                {
                                    e_comboDistrict.SelectedIndex = 0;
                                }
                                else if (reader.GetString(7).Equals("Муминобод"))
                                {
                                    e_comboDistrict.SelectedIndex = 1;
                                }
                                else if (reader.GetString(7).Equals("Дигар"))
                                {
                                    e_comboDistrict.SelectedIndex = 2;
                                }
                            }

                            if (!reader.IsDBNull(8)) e_comboJamoat.SelectedItem = reader.GetString(8);
                            if (!reader.IsDBNull(9)) e_textVillage.Text = reader.GetString(9);
                            if (!reader.IsDBNull(10)) e_textPhone.Text = reader.GetString(10);

                            if (!reader.IsDBNull(11))
                            {
                                if (reader.GetByte(11) == 1) e_checkCoffee.Checked = true;
                                else e_checkCoffee.Checked = false;
                            }

                            if (!reader.IsDBNull(12))
                            {
                                if (reader.GetByte(12) == 1) e_checkLunch.Checked = true;
                                else e_checkLunch.Checked = false;
                            }

                            if (!reader.IsDBNull(13))
                            {
                                if (reader.GetByte(13) == 1) e_checkStationary.Checked = true;
                                else e_checkStationary.Checked = false;
                            }
                            if (!reader.IsDBNull(14)) e_textTransportCost.Text = reader.GetDecimal(14).ToString();
                            if (!reader.IsDBNull(15)) e_textAccommodation.Text = reader.GetDecimal(15).ToString();
                        }
                    }
                    connection.Close();
                }
            }
        }

        private void idField_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(idField.Text, "[^0-9.]"))
            {
                MessageBox.Show(msgNumber, "Хатогӣ рӯй дод", MessageBoxButtons.OK, MessageBoxIcon.Error);
                idField.Text = idField.Text.Remove(idField.Text.Length - 1);
            }
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

        private void e_comboDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = "";
            jamoatName = "";
            if (e_comboDistrict.Text == "Дигар")
            {
                e_labelVillage.Text = "Дигар";
                districtName = "Дигар";
                jamoatName = "Дигар";
                e_comboJamoat.Enabled = false;
            }
            else
            {
                e_comboJamoat.Enabled = true;
                e_labelVillage.Text = "Деҳа";
                if (e_comboDistrict.SelectedIndex == 0)
                {
                    name = "lakhsh"; districtName = "Лахш";
                }
                else if (e_comboDistrict.SelectedIndex == 1)
                {
                    name = "muminobod"; districtName = "Муминобод";
                }
                selectLocation(name, e_comboJamoat);
            }
        }

        private void e_comboJamoat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (e_comboJamoat.Text == "Дигар")
            {
                e_labelVillage.Text = "Дигар";
            }
            else
            {
                e_labelVillage.Text = "Деҳа";
            }

            jamoatName = e_comboJamoat.SelectedItem.ToString();
        }

        private void e_comboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (e_comboType.Text == "Дигар")
            {
                e_textOther.Visible = true;
                type = "Дигар";
            }
            else if (e_comboType.Text == "Деҳқон")
            {
                e_textOther.Visible = false;
                type = "Деҳқон";
            }
            else if (e_comboType.Text == "Агроном")
            {
                e_textOther.Visible = false;
                type = "Агроном";
            }
            else
            {
                e_textOther.Visible = false;
                type = "";
            }
        }

        private void e_checkCoffee_CheckedChanged(object sender, EventArgs e)
        {
            if (e_checkCoffee.Checked)
            {
                e_checkCoffee.Text = " Ҳаст";
                coffeeCheck = 1;
            }
            else
            {
                e_checkCoffee.Text = e_checkCoffee.Text = " Нест";
                coffeeCheck = 0;
            }
        }

        private void e_checkLunch_CheckedChanged(object sender, EventArgs e)
        {
            if (e_checkLunch.Checked)
            {
                e_checkLunch.Text = " Ҳаст";
                lunchCheck = 1;
            }
            else
            {
                e_checkLunch.Text = e_checkLunch.Text = " Нест";
                lunchCheck = 0;
            }
        }

        private void e_checkStationary_CheckedChanged(object sender, EventArgs e)
        {
            if (e_checkStationary.Checked)
            {
                e_checkStationary.Text = " Ҳаст";
                stationaryCheck = 1;
            }
            else
            {
                e_checkStationary.Text = e_checkStationary.Text = " Нест";
                stationaryCheck = 0;
            }
        }

        private void e_comboGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (e_comboGender.Text == "Мард")
            {
                gender = "Мард";
            }
            else if (e_comboGender.Text == "Зан")
            {
                gender = "Зан";
            }
            else
            {
                gender = "";
            }
        }

        private void EditForm_Load(object sender, EventArgs e)
        {
            if (CultureInfo.CurrentCulture.Name.StartsWith("tg") == true)
            {
                msgNumber = "Лутфан танҳо рақамҳоро ворид кунед!";
                message = "Агар маълумоти воридшуда дуруст бошад, 'Ok'-ро пахш кунед, вагарна 'Cancel'-ро пахш кунед.";
                caption = "Таҳрири маълумот";
                messageDel = "Шумо маълумотро бо раками сатрии додашуда нест карданӣ ҳастед. Ин раванд бебозгашт аст. Оё шумо мехоҳед ин равандро идома диҳед? Агар рози бошед, 'Yes'-ро пахш кунед, вагарна 'No'-ро пахш кунед.";
                captionDel = "Тоза кардани маълумот";
                msgDeleted = "  Маълумот бомуваффақият нест карда шуд!";
                msgNotExist = "  Рақами пешниҳодшуда вуҷуд надорад!";
                msgUpdated = "  Маълумот бомуваффақият нав карда шуд!";
            }
            else 
            {
                msgNumber = "Please enter only numbers!";
                message = "You are about to edit data on the database. If the entered data is correct, click 'Ok', otherwise, click 'Cancel'.";
                caption = "Edit data";
                messageDel = "You are about to delete the data with the given id. This process is irreversible. Do you want to continue the process?";
                captionDel = "Delete data";
                msgDeleted = "  Data successfully Deleted!";
                msgNotExist = "  Provided ID does not exist!";
                msgUpdated = "  Data successfully updated!";
            }
        }

        private void e_textTransportCost_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(e_textTransportCost.Text, "[^0-9.,]"))
            {
                MessageBox.Show(msgNumber, "Хатогӣ руй дод", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e_textTransportCost.Text = e_textTransportCost.Text.Remove(e_textTransportCost.Text.Length - 1);
            }
        }

        private void e_textAccommodation_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(e_textAccommodation.Text, "[^0-9.,]"))
            {
                MessageBox.Show(msgNumber, "Хатогӣ руй дод", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e_textAccommodation.Text = e_textAccommodation.Text.Remove(e_textAccommodation.Text.Length - 1);
            }
        }

        private void e_textPhone_TextChanged_1(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(e_textPhone.Text, "[^0-9]"))
            {
                MessageBox.Show(msgNumber, "Хатогӣ руй дод", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e_textPhone.Text = e_textPhone.Text.Remove(e_textPhone.Text.Length - 1);
            }
        }

        private void e_textAge_TextChanged_1(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(e_textAge.Text, "[^0-9]"))
            {
                MessageBox.Show(msgNumber, "Хатогӣ руй дод", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e_textAge.Text = e_textAge.Text.Remove(e_textAge.Text.Length - 1);
            }
        }

        private void e_textQR_TextChanged_1(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(e_textQR.Text, "[^0-9]"))
            {
                MessageBox.Show(msgNumber, "Хатогӣ руй дод", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e_textQR.Text = e_textQR.Text.Remove(e_textQR.Text.Length - 1);
            }
        }
    }
}
