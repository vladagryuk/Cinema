using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Common;

namespace WindowsFormsApp1
{
    public partial class Mesto : Form
    {
        SqlCommand sclCom;
        SqlCommand sclCom3;
        SqlCommand sclCom4;
        SqlCommand sclCom5;
        SqlCommand sclCom6;

        string id_s; 
        string vib_mesto;
        int num_ryad; 
        int comfort;
        int startprice;
        string test;
        int price=0;
        int price2;
        int id_p;
        
        public Mesto(int id_pok) // go-номер сеанса, og-номер зала
        {
            InitializeComponent();
            id_p = id_pok;
        }
        private void LoadKolvo()
        {
            listBox2.Items.Clear();
            SqlConnection conection = ClassSQL.GetConect();
            conection.Open();

            
            string cs_date = "select * from place where free=0 and row_place="+ num_ryad.ToString()+" and session_id=" + ViborSeans.idha + ";"; 
            sclCom = new SqlCommand(cs_date, conection);
            DataSet datset = new DataSet("place"); 
            SqlDataAdapter dat = new SqlDataAdapter(sclCom);
            dat.Fill(datset, "place");
            foreach (DataRow row in datset.Tables["place"].Rows)
            {
                listBox2.Items.Add(row["place"]);
            }
            conection.Close();
            conection.Dispose();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            vib_mesto = listBox2.SelectedItem.ToString();
            button1.Visible = true;
            button2.Visible = true;
            label5.Visible = true;
            label6.Visible = true;
            label4.Visible = true;
            label3.Visible = true;

            //ПРОВЕРЯЕМ КОМФОРТ
            string sc_getBal = " select comfort from place where session_id=" + ViborSeans.idha + " and row_place=" + num_ryad + " and place=" + vib_mesto + ";";
            SqlConnection conection2 = ClassSQL.GetConect();
            conection2.Open();
            sclCom3 = new SqlCommand(sc_getBal, conection2);
            test = sclCom3.ExecuteScalar().ToString();

            comfort = Int32.Parse(test);
            if (comfort != 1)
            {
                label6.Text = "Нет";
            }
            else
            {
                label6.Text = "Да";
                price = 100;
            }

            //ПРОВЕРЯЕМ НАЧ ЦЕНУ ОТ ФИЛЬМА
            string sc_getStart = "select start_price from film f join film_session s on f.film_id=s.film_id and s.session_id=" + ViborSeans.idha + ";";
            SqlConnection conection3 = ClassSQL.GetConect();
            conection3.Open();
            sclCom4 = new SqlCommand(sc_getStart, conection2);
            test = sclCom4.ExecuteScalar().ToString();
            startprice = int.Parse(test);
            price += startprice;

            //ПРОВЕРЯЕМ ВРЕМЯ СЕАНСА
            string sc_getTime = "set language english;select case when ((select convert(varchar(8),date_session,108) from film_session where session_id=" + ViborSeans.idha + ") between '18:00:00' and '5:00:00') then 1 else 0 end  ";
            SqlConnection conection7 = ClassSQL.GetConect();
            conection7.Open();
            sclCom6 = new SqlCommand(sc_getTime, conection7);
            string w = sclCom6.ExecuteScalar().ToString();
            if (w=="1")
            {
                price += 70;
            }

            //ПРОВЕРЯЕМ ЕСТЬ ПРЕМЬЕРА
            string sc_getPrem = "SELECT case when (select f.end_of_premiere from film_session s join film f on f.film_id=s.film_id and s.session_id=" + ViborSeans.idha + ")> getdate() or (select f.release_date from film_session s join film f on f.film_id=s.film_id and s.session_id=" + ViborSeans.idha + ")= getdate() then 1 else 0 end";
            SqlConnection conection4 = ClassSQL.GetConect();
            conection4.Open();
            sclCom5 = new SqlCommand(sc_getPrem, conection4);
            string a = sclCom5.ExecuteScalar().ToString();
            if (a=="1")
            {
                price += 115;
            }

            //ФОРМИРУЕМ ЦЕНУ
            label3.Text = price.ToString();
            price2 = price;
            price = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form Oplata = new Oplata(vib_mesto,price2,id_p,num_ryad);
            Oplata.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string cmdText;
            string cmdText2;
            SqlConnection conection = ClassSQL.GetConect();
            SqlConnection conectionn = ClassSQL.GetConect();
            conectionn.Open();
            conection.Open(); 
            cmdText = "insert into ticket(session_id, place_id, date_ticket, time_ticket, price, payment) values(" + ViborSeans.idha + "," + vib_mesto + ",convert(char(10),getdate(),102),convert(char(10),getdate(),108)," + price2 + ",0);";
            cmdText2 = "update place set free = 1  where place =" + vib_mesto + " and row_place ="+num_ryad+" and session_id = " + ViborSeans.idha + ";";
            try
            {
                sclCom4 = new SqlCommand(cmdText, conection);
                sclCom4.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка системы. ", "Ошибка" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                try
                {
                    sclCom5 = new SqlCommand(cmdText2, conectionn);
                    sclCom5.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка системы. ", "Ошибка" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    int t;
                    Random r = new Random();
                    t = (r.Next(10000, 99999));

                    conection.Close();
                    conection.Dispose();
                    conectionn.Close();
                    conectionn.Dispose();
                    MessageBox.Show("Ваше место забронировано.", "Уведомление", MessageBoxButtons.OK,MessageBoxIcon.Information);
                    MessageBox.Show("Ваше забронированое место: " + vib_mesto + "\nСкажите на кассе ваш номер:  " + t, "Уведомление", MessageBoxButtons.OK,MessageBoxIcon.Information);
                    this.Hide();
                }
            }
        }


        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            num_ryad = 4;
            LoadKolvo();
        }

        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {
            num_ryad = 1;
            LoadKolvo();
        }

        private void radioButton2_CheckedChanged_1(object sender, EventArgs e)
        {
            num_ryad = 2;
            LoadKolvo();
        }

        private void radioButton3_CheckedChanged_1(object sender, EventArgs e)
        {
            num_ryad = 3;
            LoadKolvo();
        }

        private void radioButton4_CheckedChanged_1(object sender, EventArgs e)
        {
            num_ryad = 4;
            LoadKolvo();
        }
    }
}

