using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Common;

namespace WindowsFormsApp1
{
    public partial class PersFrom : Form
    {
        SqlCommand scom;
        string sc_getName = "select p.personal_id, c.cinema_name,c.street,c.house, p.position, p.surname_pers, p.name_pers, p.patronymic_pers, p.passport_seria, p.passport_number, p.phone_pers from personal p join cinema c on c.cinema_id = p.cinema_id;";
        public static int id_p; 
        public static string id_d;
        string id_pol; 
        SqlConnection conection;
        public PersFrom(int id_pers, string id_dolj) //string
        {
            
            InitializeComponent();
            id_p = id_pers;
            id_d = id_dolj;
            id_pol = id_dolj;
            conection = ClassSQL.GetConect();
            conection.Open();
            try
            {
                    FillInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка системы. ", "Ошибка"+ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conection.Close();
                conection.Dispose();
            }
            if (id_d == "администратор") 
            {
                button7.Visible = true;
                button3.Visible = true;
                button4.Visible = true;
                panel1.Location = new System.Drawing.Point(12, 190);
            }




        }

        private void FillInfo()
        {
            scom = new SqlCommand(sc_getName, conection);
            using (DbDataReader reader = scom.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (id_p == reader.GetInt32(0)) 
                        {
                            label_cinema.Text = reader.GetString(1);
                            label_adress.Text = reader.GetString(2) + " д." + reader.GetString(3);
                            label_position.Text = reader.GetString(4);
                            label_FIO.Text = reader.GetString(5) + " " + reader.GetString(6) + " " + reader.GetString(7);
                            label_passport.Text = reader.GetString(8) + " " + reader.GetString(9);

                        }
                    }

                }
            }

        }

        private void завершитьРаботуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e) 
        {
            if (id_d == "администратор")
            {
                id_p = -1;
            }
            else
            {
                id_p = 0;
            }
            Form ViborSeans = new ViborSeans(id_p); 
            ViborSeans.Show();
            this.Hide();
        }

        private void PersFrom_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }



        private void button4_Click(object sender, EventArgs e)
        {
            Form AddFilm = new AddFilm();
            AddFilm.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form AddSeans = new AddSeans();
            AddSeans.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
            Form AddPrav = new AddPrav();
            AddPrav.Show();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            
            Form AddUser = new AddUser();
            AddUser.Show();
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            Form Otchet_vir = new Otchet_vir();
            Otchet_vir.Show();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Form Otchet_bilet = new Otchet_bilet();
            Otchet_bilet.Show();
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form Spravka = new Spravka();
            Spravka.Show();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            Form AddSeans = new AddSeans();
            AddSeans.Show();
        }

        private void поменятьПользователяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form BeginFormAuto = new BeginFormAuto();
            BeginFormAuto.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form FormPokup = new PokupForm(0, 0);
            FormPokup.Show();
        }
    }
}
