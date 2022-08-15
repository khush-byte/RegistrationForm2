using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegistrationForm
{
    public partial class Form1 : Form
    {
        string connString = @"Data Source=(localdb)\MSSQLLocalDB; AttachDbFilename=|DataDirectory|\Database.mdf; Integrated Security=True;";
        string date;
        string other = "";
        string gender = "";
        string type = "";
        decimal accommodation = 0;
        decimal transportationCost = 0;
        int lunchCheck = 0;
        int stationaryCheck = 0;
        int supperCheck = 0;
        int coffeeCheck = 0;
        int age = -1;
        string projectName = "";
        string locationName = "";
        string locationName2 = "";
        string districtName = "";
        string jamoatName = "";
        string categoryName = "";
        string topicName = "";
        string trainerName = "";
        int number = 1;
        DataTable sendTable = new DataTable();
        string msgNumber, message, caption, messageDel, msgUpdated, msgAdd;
        string captionDel, msgNotExist, msgDeleted, msgSubmitted, msgFill, msgDuplicat;

        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            date = DateTime.Now.ToString("dd.MM.yyyy");
            date_field.Text = date;
        }

        /*private void trainingBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.trainingBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.databaseDataSet);

        }*/

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'databaseDataSet.Training' table. You can move, or remove it, as needed.
            //this.trainingTableAdapter.Fill(this.databaseDataSet.Training);
            InitializeDataGridView();

            if (CultureInfo.CurrentCulture.Name.StartsWith("tg") == true)
            {
                msgNumber = "Лутфан танҳо рақамҳоро ворид кунед!";
                message = "Шумо маълумоти нав илова карда истодаед.\nАгар маълумоти воридшуда дуруст бошад, 'Ok'-ро пахш кунед, вагарна 'Cancel'-ро пахш кунед.";
                caption = "Тасдиқи маълумот";
                messageDel = "Шумо маълумотро бо раками сатрии додашуда нест карданӣ ҳастед. Ин раванд бебозгашт аст. Оё шумо мехоҳед ин равандро идома диҳед? Агар рози бошед, 'Yes'-ро пахш кунед, вагарна 'No'-ро пахш кунед.";
                captionDel = "Тоза кардани маълумот";
                msgDeleted = "  Маълумот бомуваффақият нест карда шуд!";
                msgNotExist = "  Рақами пешниҳодшуда вуҷуд надорад!";
                msgUpdated = "  Маълумот бомуваффақият нав карда шуд!";
                msgSubmitted = "  Маълумоти нав бомуваффақият пешниҳод карда шуд!";
                msgFill = "  Лутфан ҳамаи майдонҳоро пур кунед!";
                msgAdd = "  Маълумоти нав бомуваффақият илова карда шуд!";
                msgDuplicat = "Дубликати рақами иштирокчӣ. \nШумо як рақамро ду маротиба истифода карда наметавонед!";
            }
            else
            {
                msgNumber = "Please enter only numbers!";
                message = "You are about to add new data to the database. If the entered data is correct, click 'Yes', otherwise, click 'No' and edit the data.";
                caption = "Сonfirmation Box";
                messageDel = "You are about to delete the data with the given id. This process is irreversible. Do you want to continue the process?";
                captionDel = "Delete data";
                msgDeleted = "  Data successfully Deleted!";
                msgNotExist = "  Provided ID does not exist!";
                msgUpdated = "  Data successfully updated!";
                msgSubmitted = "  New data successfully submitted!";
                msgFill = "  Please fill in all fields!";
                msgAdd = "  New data successfully added!";
                msgDuplicat = "Duplicate participant IDs. \nYou couldn't use the same ID twice!";
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            if (comboLocation.Text == "Дигар") {
                locationName = textLocation.Text.ToString().Replace("\"", "\'");
                locationName2 = textLocation2.Text.ToString().Replace("\"", "\'");
            };
            if (comboLocation2.Text == "Дигар") locationName2 = textLocation2.Text.ToString().Replace("\"", "\'");
            if (comboCategory.Text == "Дигар") categoryName = textCategory.Text.ToString().Replace("\"", "\'");
            if (comboTopic.Text == "Дигар") topicName = textTopic.Text.ToString().Replace("\"", "\'");
            if (comboTrainer.Text == "Дигар") trainerName = textTrainer.Text.ToString().Replace("\"", "\'");
            if (comboType.Text == "Дигар") type = textOther.Text.ToString().Replace("\"", "\'");

            string first_name = textParName.Text.ToString().Replace(" ", "");
            string last_name = textParName2.Text.ToString().Replace(" ", "");

            if(supperCheck==1) transportationCost = 1;
            else transportationCost = 0;

            if (textOther.Visible)
                other = textOther.Text;
            else other = "";

            string accom = textAccommodation.Text.ToString();
            accom = accom.Replace(",", ".");
            if (accom.Length > 0)
            {
                if (!accom.Equals("."))
                    accommodation = Decimal.Parse(accom, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
                else accommodation = 0;
            }
            else accommodation = 0;

            /*string trans = textTransportCost.Text.ToString();
            trans = trans.Replace(",", ".");
            if (trans.Length > 0)
            {
                if (!trans.Equals("."))
                    transportationCost = Decimal.Parse(trans, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
                else transportationCost = 0;
            }
            else transportationCost = 0;*/

            if (textAge.Text.Length > 0)
                age = Int32.Parse(textAge.Text);
            else age = -1;

            //QR check
            int countCheck = 1;

            if (textQR.Text.Length == 5)
            {
                string queryString = "select count(Id) from Training where QR = "+ textQR.Text.ToString() + " and Training.Date = '"+ date_field.Text.ToString()+ "';";
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            countCheck = reader.GetInt32(0);
                            if (countCheck > 0)
                                MessageBox.Show(msgDuplicat, "Хатогӣ рӯй дод", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                    connection.Close();
                }
            }

            if (countCheck < 1 && textParName.Text.Length>0 && comboDistrict.SelectedIndex>-1)
            {
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.OKCancel,
                                             MessageBoxIcon.Warning);

                // If the no button was pressed ...
                if (result == DialogResult.OK)
                {
                    using (var conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("sp_insert", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@date", SqlDbType.VarChar).Value = date_field.Text.ToString();
                        cmd.Parameters.Add("@project", SqlDbType.VarChar).Value = projectName;
                        cmd.Parameters.Add("@location", SqlDbType.NVarChar).Value = locationName;
                        cmd.Parameters.Add("@locationJamoat", SqlDbType.NVarChar).Value = locationName2;
                        cmd.Parameters.Add("@category", SqlDbType.NVarChar).Value = categoryName;
                        cmd.Parameters.Add("@topic", SqlDbType.NVarChar).Value = topicName;
                        cmd.Parameters.Add("@trainer", SqlDbType.NVarChar).Value = trainerName;

                        cmd.Parameters.Add("@qr", SqlDbType.VarChar).Value = textQR.Text.ToString();
                        cmd.Parameters.Add("@parNameFirst", SqlDbType.NVarChar).Value = first_name.Replace("\"", "\'");
                        cmd.Parameters.Add("@parNameLast", SqlDbType.NVarChar).Value = last_name.Replace("\"", "\'");
                        cmd.Parameters.Add("@gender", SqlDbType.NVarChar).Value = gender;
                        cmd.Parameters.Add("@passport", SqlDbType.NVarChar).Value = textPassport.Text.ToString().Replace("\"", "\'");
                        cmd.Parameters.Add("@age", SqlDbType.VarChar).Value = age;
                        cmd.Parameters.Add("@type", SqlDbType.NVarChar).Value = type;
                        cmd.Parameters.Add("@other", SqlDbType.VarChar).Value = "";
                        cmd.Parameters.Add("@district", SqlDbType.NVarChar).Value = districtName;
                        cmd.Parameters.Add("@jamoat", SqlDbType.NVarChar).Value = jamoatName;
                        cmd.Parameters.Add("@village", SqlDbType.NVarChar).Value = textVillage.Text.ToString().Replace("\"", "\'");
                        cmd.Parameters.Add("@phone", SqlDbType.VarChar).Value = textPhone.Text.ToString();
                        cmd.Parameters.Add("@coffee", SqlDbType.VarChar).Value = coffeeCheck;
                        cmd.Parameters.Add("@lunch", SqlDbType.VarChar).Value = lunchCheck;
                        cmd.Parameters.Add("@stationary", SqlDbType.VarChar).Value = stationaryCheck;
                        cmd.Parameters.Add("@transport", SqlDbType.Decimal).Value = transportationCost;
                        cmd.Parameters.Add("@accom", SqlDbType.Decimal).Value = accommodation;
                        cmd.Parameters.Add("@number", SqlDbType.VarChar).Value = number;
                        cmd.Parameters.Add("@supper", SqlDbType.VarChar).Value = supperCheck;

                        int i = cmd.ExecuteNonQuery();
                        if (i != 0)
                        {
                            MessageBox.Show(msgAdd, "Хабар", MessageBoxButtons.OK, MessageBoxIcon.None);
                            number += 1;
                            InitializeDataGridView();
                        }
                        conn.Close();
                    }
                }

                textQR.Clear();
                textParName.Clear();
                textPassport.Clear();
                textAge.Clear();
                comboGender.SelectedIndex = -1;
                comboType.SelectedIndex = -1;
                comboDistrict.SelectedIndex = -1;
                comboJamoat.Items.Clear();
                textVillage.Clear();
                textPhone.Clear();
                checkCoffee.Checked = false;
                checkLunch.Checked = false;
                checkSupper.Checked = false;
                checkStationary.Checked = false;
                textAccommodation.Clear();
                textParName2.Clear();
            }
            else MessageBox.Show(msgFill, "Хатогӣ рӯй дод", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private void InitializeDataGridView()
        {
            this.trainingDataGridView.DataSource = null;
            this.trainingDataGridView.Rows.Clear();

            using (SqlConnection con = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Training WHERE Date = '" + date_field.Text.ToString()+"';", con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            //sendTable = dt;
                            sda.Fill(dt);
                            trainingDataGridView.DataSource = dt;
                            if(trainingDataGridView.RowCount>0)
                            trainingDataGridView.FirstDisplayedScrollingRowIndex = trainingDataGridView.RowCount - 1;
                        }
                    }
                    con.Close();
                }
            }
        }

        private void checkCoffee_CheckedChanged(object sender, EventArgs e)
        {
            if (checkCoffee.Checked)
            {
                checkCoffee.Text = " Хаст";
                coffeeCheck = 1;
            }
            else
            {
                checkCoffee.Text = " Нест";
                coffeeCheck = 0;

            }
        }

        private void checkLunch_CheckedChanged(object sender, EventArgs e)
        {
            if (checkLunch.Checked)
            {
                checkLunch.Text = " Хаст";
                lunchCheck = 1;
            }
            else
            {
                checkLunch.Text = " Нест";
                lunchCheck = 0;
            }
        }

        private void checkSupper_CheckedChanged(object sender, EventArgs e)
        {
            if (checkSupper.Checked)
            {
                checkSupper.Text = " Хаст";
                supperCheck = 1;
            }
            else
            {
                checkSupper.Text = " Нест";
                supperCheck = 0;
            }
        }

        private void comboGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboGender.Text == "Мард")
            {
                gender = "Мард";
            }
            else if (comboGender.Text == "Зан")
            {
                gender = "Зан";
            }
            else
            {
                gender = "";
            }
        }

        private void textAge_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textAge.Text, "[^0-9]"))
            {
                MessageBox.Show(msgNumber, "Хатогӣ рӯй дод", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textAge.Text = textAge.Text.Remove(textAge.Text.Length - 1);
            }
        }

        private void textQR_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textQR.Text, "[^0-9]"))
            {
                MessageBox.Show(msgNumber, "Хатогӣ рӯй дод", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textQR.Text = textQR.Text.Remove(textQR.Text.Length - 1);
            }

            if (textQR.Text.Length > 4)
            {
                string queryString = "SELECT ParticipantName, SecondName, Gender, PassportID, Age, ParticipantType, Address, jamoat, village, Phone FROM Participant where QR = " + textQR.Text.ToString();
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {                           
                            if (!reader.IsDBNull(0)) textParName.Text = reader.GetString(0);
                            if (!reader.IsDBNull(1)) textParName2.Text = reader.GetString(1);

                            if (!reader.IsDBNull(2))
                            {
                                if (reader.GetString(2).Equals("Мард")) comboGender.SelectedIndex = 0;
                                else if (reader.GetString(2).Equals("Зан")) comboGender.SelectedIndex = 1;
                            }

                            if (!reader.IsDBNull(3)) textPassport.Text = reader.GetString(3);
                            if (!reader.IsDBNull(4)) textAge.Text = reader.GetInt32(4).ToString();

                            if (!reader.IsDBNull(5))
                            {
                                if (reader.GetString(5).Equals("Деҳқон")) comboType.SelectedIndex = 0;
                                else if (reader.GetString(5).Equals("Агроном")) comboType.SelectedIndex = 1;
                                else if (!reader.GetString(5).Equals(""))
                                {
                                    comboType.SelectedIndex = 2;
                                    if (!reader.IsDBNull(5)) textOther.Text = reader.GetString(5);
                                }
                            }

                            if (!reader.IsDBNull(6))
                            {
                                if (reader.GetString(6).Equals("Лахш"))
                                {
                                    comboDistrict.SelectedIndex = 0;
                                }
                                else if (reader.GetString(6).Equals("Муминобод"))
                                {
                                    comboDistrict.SelectedIndex = 1;
                                }
                                else if (reader.GetString(6).Equals("Дигар"))
                                {
                                    comboDistrict.SelectedIndex = 2;
                                }
                            }

                            if (!reader.IsDBNull(7)) comboJamoat.SelectedItem = reader.GetString(7);
                            if (!reader.IsDBNull(8)) textVillage.Text = reader.GetString(8);
                            if (!reader.IsDBNull(9)) textPhone.Text = reader.GetString(9);
                        }
                    }
                    connection.Close();
                }
            }
        }

        private void buttonPrint_Click(object sender, EventArgs e)
        {
            if (comboLocation.Text == "Дигар")
            {
                locationName = textLocation.Text;
                locationName2 = textLocation2.Text;
            };
            if (comboLocation2.Text == "Дигар") locationName2 = textLocation2.Text;
            if (comboCategory.Text == "Дигар") categoryName = textCategory.Text;
            if (comboTopic.Text == "Дигар") topicName = textTopic.Text;
            if (comboTrainer.Text == "Дигар") trainerName = textTrainer.Text;

            float width_ratio = Screen.PrimaryScreen.Bounds.Width;
            float heigh_ratio = Screen.PrimaryScreen.Bounds.Height;           

            if (width_ratio == 1920 && heigh_ratio == 1080)
            {
                Form2 form = new Form2();
                string new_location = locationName;
                form.SetValues(date_field.Text.ToString(), projectName, locationName, categoryName, topicName, trainerName);
                form.Show();
            }
            else 
            {
                MessageBox.Show("Андоза экран мувофиқ нест! Бояд 1920 ба 1080 бошад.", "Хатогӣ рӯй дод", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        /*private void textTransportCost_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textTransportCost.Text, "[^0-9.,]"))
            {
                MessageBox.Show(msgNumber, "Хатогӣ рӯй дод", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textTransportCost.Text = textTransportCost.Text.Remove(textTransportCost.Text.Length - 1);
            }
        }*/

        private void textAccommodation_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textAccommodation.Text, "[^0-9.,]"))
            {
                MessageBox.Show(msgNumber, "Хатогӣ рӯй дод", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textAccommodation.Text = textAccommodation.Text.Remove(textAccommodation.Text.Length - 1);
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            EditForm f2 = new EditForm();
            f2.Show();
            f2.Closed += F2_Closed;
        }

        private void comboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboType.Text == "Дигар")
            {
                textOther.Visible = true;
                type = "Дигар";
            }
            else if (comboType.Text == "Деҳқон")
            {
                textOther.Visible = false;
                type = "Деҳқон";
            }
            else if (comboType.Text == "Агроном")
            {
                textOther.Visible = false;
                type = "Агроном";
            }
            else
            {
                textOther.Visible = false;
                type = "";
            }
        }

        private void F2_Closed(object sender, EventArgs e)
        {
            InitializeDataGridView();
        }

        private void comboTrainer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboTrainer.Text == "Дигар")
            {
                textTrainer.Visible = true;
            }
            else
            {
                textTrainer.Visible = false;
            }

            trainerName = comboTrainer.SelectedItem.ToString();
        }

        private void comboProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboProject.SelectedIndex == 0)
            {
                projectName = "WWCS";
            }
            else if (comboProject.SelectedIndex == 1)
            {
                projectName = "WRRC";
            }
            else
            {
                projectName = "";
            }
        }

        private void comboLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboLocation.Text == "Дигар")
            {
                textLocation.Visible = true;
                textLocation2.Visible = true;
                comboLocation2.Enabled = false;
            }
            else
            {
                textLocation.Visible = false;
                textLocation2.Visible = false;
                comboLocation2.Enabled = true;
                string name = "";
                if (comboLocation.SelectedIndex == 0) name = "lakhsh";
                if (comboLocation.SelectedIndex == 1) name = "muminobod";
                selectLocation(name, comboLocation2);
            }

            locationName = comboLocation.SelectedItem.ToString();
        }

        private void comboLocation2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboLocation2.Text == "Дигар")
            {
                textLocation2.Visible = true;
            }
            else
            {
                textLocation2.Visible = false;
            }

            locationName2 = comboLocation2.SelectedItem.ToString();
        }

        private void comboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboCategory.Text == "Дигар")
            {
                textCategory.Visible = true;
            }
            else
            {
                textCategory.Visible = false;
            }

            categoryName = comboCategory.SelectedItem.ToString();
        }

        private void comboTopic_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboTopic.Text == "Дигар")
            {
                textTopic.Visible = true;
            }
            else
            {
                textTopic.Visible = false;
            }

            topicName = comboTopic.SelectedItem.ToString();
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

        private void comboDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = "";
            jamoatName = "";
            if (comboDistrict.Text == "Дигар")
            {
                labelVillage.Text = "Дигар";
                districtName = "Дигар";
                jamoatName = "Дигар";
                comboJamoat.Enabled = false;
            }
            else
            {
                comboJamoat.Enabled = true;
                labelVillage.Text = "Деҳа";
                if (comboDistrict.SelectedIndex == 0) { 
                    name = "lakhsh"; districtName = "Лахш"; 
                }
                else if (comboDistrict.SelectedIndex == 1) { 
                    name = "muminobod"; districtName = "Муминобод"; 
                }
                selectLocation(name, comboJamoat);
            }   
        }

        private void comboJamoat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboJamoat.Text == "Дигар")
            {
                labelVillage.Text = "Дигар";
            }
            else
            {
                labelVillage.Text = "Деҳа";
            }

            jamoatName = comboJamoat.SelectedItem.ToString();
        }

        private void textPhone_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textPhone.Text, "[^0-9]"))
            {
                MessageBox.Show(msgNumber, "Хатогӣ рӯй дод", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textPhone.Text = textPhone.Text.Remove(textPhone.Text.Length - 1);
            }
        }

        private void checkStationary_CheckedChanged(object sender, EventArgs e)
        {
            if (checkStationary.Checked)
            {
                checkStationary.Text = " Хаст";
                stationaryCheck = 1;
            }
            else
            {
                checkStationary.Text = " Нест";
                stationaryCheck = 0;
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (CheckConnection())
            {
                buttonUpdate.Text = "ИҶРОИШ...";
                GetList();
            }
            else
            {
                MessageBox.Show(" Интернет пайваст нест!\n Барои ин амал интернет лозим аст.", "Хатогӣ руй дод", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        public async void GetList()
        {
            await Task.Delay(2000);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://wwcs.tj/meteo/regform/list.php");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            try
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"sign\":\"21e6d5126bcdd0c7606c86fa054da874\"," +
                                  "\"datetime\":\"2021-09-30 15:45:47\"}";

                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();

                    if (result.Length > 0)
                    {
                        try
                        {
                            using (var conn = new SqlConnection(connString))
                            {
                                conn.Open();
                                SqlCommand cmd = new SqlCommand("sp_clean", conn);
                                cmd.CommandType = CommandType.StoredProcedure;
                                int i = cmd.ExecuteNonQuery();
                                conn.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            buttonUpdate.Text = "АЗНАВСОЗӢ";
                            MessageBox.Show(ex.ToString(), "Хатогӣ руй дод", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }

                        try
                        {
                            JArray array = JArray.Parse(result);
                            for (int j = 0; j < array.Count; j++)
                            {
                                var json = array[j].ToString();
                                RegList data = JsonConvert.DeserializeObject<RegList>(json);
                                //MessageBox.Show(data.firstName, "Message", MessageBoxButtons.OK, MessageBoxIcon.None);

                                using (var conn = new SqlConnection(connString))
                                {
                                    conn.Open();
                                    SqlCommand cmd = new SqlCommand("sp_update", conn);
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    if (data.age.Length > 0) age = Int32.Parse(data.age);
                                    cmd.Parameters.Add("@qr", SqlDbType.VarChar).Value = data.QR;
                                    cmd.Parameters.Add("@parNameFirst", SqlDbType.NVarChar).Value = data.firstName;
                                    cmd.Parameters.Add("@parNameLast", SqlDbType.NVarChar).Value = data.lastName;
                                    cmd.Parameters.Add("@gender", SqlDbType.NVarChar).Value = data.gender;
                                    cmd.Parameters.Add("@passport", SqlDbType.NVarChar).Value = data.passportID;
                                    cmd.Parameters.Add("@age", SqlDbType.Int).Value = age;
                                    cmd.Parameters.Add("@type", SqlDbType.NVarChar).Value = data.type;
                                    cmd.Parameters.Add("@district", SqlDbType.NVarChar).Value = data.district;
                                    cmd.Parameters.Add("@jamoat", SqlDbType.NVarChar).Value = data.jamoat;
                                    cmd.Parameters.Add("@village", SqlDbType.NVarChar).Value = data.village;
                                    cmd.Parameters.Add("@phone", SqlDbType.NVarChar).Value = data.phone;

                                    int i = cmd.ExecuteNonQuery();
                                    conn.Close();
                                }
                            }

                            buttonUpdate.Text = "АЗНАВСОЗӢ";
                            MessageBox.Show(msgUpdated, "Хабар", MessageBoxButtons.OK, MessageBoxIcon.None);
                        }
                        catch (Exception ex)
                        {
                            buttonUpdate.Text = "АЗНАВСОЗӢ";
                            MessageBox.Show(ex.ToString(), "Хатогӣ руй дод", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                buttonUpdate.Text = "АЗНАВСОЗӢ";
                MessageBox.Show(" Интернет пайваст нест! \n Барои ин амал интернет лозим аст.", "Хатогӣ руй дод", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        public async void SendData(List<SubmitData> array)
        {
            bool isSend = false;

            for (int i = 0; i < array.Count; i++)
            {
                if (array[i].status.Equals("0"))
                {
                    await Task.Delay(1000);
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://wwcs.tj/meteo/regform/insertdata.php");
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    
                    try
                    {
                        string hash = CreateMD5(array[i].qr + array[i].date + "bCctS9eqoYaZl21a");
                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string accom = "0";
                            string trans = "0";
                            if(!array[i].accommodation.Equals("")) accom = array[i].accommodation.Replace(",", ".");
                            if (!array[i].transport.Equals("")) trans = array[i].transport.Replace(",", ".");
                            string json = "{\"sign\":\"" + hash + "\"," +
                                "\"qr\":\"" + array[i].qr + "\"," +
                                "\"firstName\":\"" + array[i].firstName + "\"," +
                                "\"lastName\":\"" + array[i].lastName + "\"," +
                                "\"gender\":\"" + array[i].gender + "\"," +
                                "\"passport\":\"" + array[i].passport + "\"," +
                                "\"age\":\"" + array[i].age + "\"," +
                                "\"type\":\"" + array[i].type + "\"," +
                                "\"district\":\"" + array[i].district + "\"," +
                                "\"jamoat\":\"" + array[i].jamoat + "\"," +
                                "\"village\":\"" + array[i].village + "\"," +
                                "\"phone\":\"" + array[i].phone + "\"," +
                                "\"coffee\":\"" + array[i].coffee + "\"," +
                                "\"lunch\":\"" + array[i].lunch + "\"," +
                                "\"stationary\":\"" + array[i].stationary + "\"," +
                                "\"transport\":\"" + trans + "\"," +
                                "\"accommodation\":\"" + accom + "\"," +
                                "\"project\":\"" + array[i].project + "\"," +
                                "\"eventDistrict\":\"" + array[i].eventDistrict + "\"," +
                                "\"eventJamoat\":\"" + array[i].eventJamoat + "\"," +
                                "\"category\":\"" + array[i].category + "\"," +
                                "\"topic\":\"" + array[i].topic + "\"," +
                                "\"trainer\":\"" + array[i].trainer + "\"," +
                                "\"date\":\"" + array[i].date + "\"}";

                            streamWriter.Write(json);
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            string result = streamReader.ReadToEnd();
                            streamReader.Close();

                            int newID = Int32.Parse(array[i].id);
                            updateStatus(newID);
                            isSend = true;
                            //MessageBox.Show(result, "Хабар", MessageBoxButtons.OK, MessageBoxIcon.None);
                        }
                    }
                    catch (Exception ex)
                    {
                        isSend = false;
                        buttonSubmit.Text = "ПЕШНИҲОД";
                        i = array.Count;
                        MessageBox.Show(" Интернет пайваст нест!\n Барои ин амал интернет лозим аст.", "Хатогӣ руй дод", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }               
            }
            buttonSubmit.Text = "ПЕШНИҲОД";
            if (isSend)
            {
                MessageBox.Show(msgSubmitted, "Хабар", MessageBoxButtons.OK, MessageBoxIcon.None);
                InitializeDataGridView();
            }
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            if (CheckConnection())
            {
                buttonSubmit.Text = "ИҶРОИШ...";
                List<SubmitData> array = new List<SubmitData>();

                string age, firstName, lastName, gender, transport, accommodation, passport, type, district, jamoat, village, phone, trainer, topic, category, eventJamoat, eventDistrict, project;
                string queryString = "SELECT * FROM Training where Status = 0;";
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(7)) age = reader.GetInt32(7).ToString(); else age = "";
                            if (!reader.IsDBNull(18)) transport = reader.GetDecimal(18).ToString(); else transport = "";
                            if (!reader.IsDBNull(19)) accommodation = reader.GetDecimal(19).ToString(); else accommodation = "";
                            if (!reader.IsDBNull(2)) firstName = reader.GetString(2); else firstName = "";
                            if (!reader.IsDBNull(3)) lastName = reader.GetString(3); else lastName = "";
                            if (!reader.IsDBNull(5)) gender = reader.GetString(5); else gender = "";
                            if (!reader.IsDBNull(6)) passport = reader.GetString(6); else passport = "";
                            if (!reader.IsDBNull(8)) type = reader.GetString(8); else type = "";
                            if (!reader.IsDBNull(10)) district = reader.GetString(10); else district = "";
                            if (!reader.IsDBNull(11)) jamoat = reader.GetString(11); else jamoat = "";
                            if (!reader.IsDBNull(12)) village = reader.GetString(12); else village = "";
                            if (!reader.IsDBNull(14)) phone = reader.GetString(14); else phone = "";
                            if (!reader.IsDBNull(26)) trainer = reader.GetString(26); else trainer = "";
                            if (!reader.IsDBNull(25)) topic = reader.GetString(25); else topic = "";
                            if (!reader.IsDBNull(24)) category = reader.GetString(24); else category = "";
                            if (!reader.IsDBNull(23)) eventJamoat = reader.GetString(23); else eventJamoat = "";
                            if (!reader.IsDBNull(22)) eventDistrict = reader.GetString(22); else eventDistrict = "";
                            if (!reader.IsDBNull(21)) project = reader.GetString(21); else project = null;

                            SubmitData data = new SubmitData();
                            data.id = reader.GetInt32(0).ToString();
                            data.qr = reader.GetString(1);
                            data.firstName = firstName;
                            data.lastName = lastName;
                            data.gender = gender;
                            data.passport = passport;
                            data.age = age;
                            data.type = type;
                            data.district = district;
                            data.jamoat = jamoat;
                            data.village = village;
                            data.phone = phone;
                            data.coffee = reader.GetByte(15).ToString();
                            data.lunch = reader.GetByte(16).ToString();
                            data.stationary = reader.GetByte(17).ToString();
                            data.transport = transport;
                            data.accommodation = accommodation;
                            data.date = reader.GetString(20);
                            data.trainer = trainer;
                            data.topic = topic;
                            data.category = category;
                            data.eventJamoat = eventJamoat;
                            data.eventDistrict = eventDistrict;
                            data.project = project;
                            data.status = reader.GetByte(27).ToString();
                            array.Add(data);
                        }
                    }
                    connection.Close();
                }

                SendData(array);
            }
            else 
            {
                MessageBox.Show(" Интернет пайваст нест!\n Барои ин амал интернет лозим аст.", "Хатогӣ руй дод", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString().ToLower();
            }
        }

        private void date_field_TextChanged(object sender, EventArgs e)
        {
            InitializeDataGridView();
        }

        private void updateStatus(int id) {
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("status", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        private void btnTaj_Click(object sender, EventArgs e)
        {
            var changeLanguage = new ChangeLanguage();
            changeLanguage.UpdateConfig("language", "tg");
            Application.Restart();
        }

        private void btnEng_Click(object sender, EventArgs e)
        {

            var changeLanguage = new ChangeLanguage();
            changeLanguage.UpdateConfig("language", "en");
            Application.Restart();
        }

        public static bool CheckConnection()
        {
            try
            {
                Ping myPing = new Ping();
                String host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                //return (reply.Status == IPStatus.Success);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
