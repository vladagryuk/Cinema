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

    public partial class ViborSeans : Form
    {
        SqlCommand sclCom;
        SqlCommand sclCom2;
        SqlCommand sclCom3;
        SqlCommand sclCom4;
        SqlCommand sclCom5;

        string sc_getFilm = "Select film_name from film";
        
        int Filmc;
        SqlConnection conection;
        int id_p;
        string go;
        string og;
        public static string idha;

        public ViborSeans(int id_pok)
        {
            InitializeComponent();
            id_p = id_pok;
            conection = ClassSQL.GetConect();
            conection.Open();
            try
            {
                sclCom = new SqlCommand(sc_getFilm, conection);
                DataSet ds = new DataSet("film_name");
                SqlDataAdapter dat = new SqlDataAdapter(sclCom);
                dat.Fill(ds, "Film");
                foreach (DataRow row in ds.Tables["Film"].Rows)
                {
                    listBox1.Items.Add(row["film_name"]);
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show("Ошибка системы. ","Ошибка" + ex.Message,MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conection.Close();
                conection.Dispose();
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            Filmc = listBox1.SelectedIndex + 10;
            string sc_getDate = "Select date_session from film_session where date_session >= getdate() and film_id=" + Filmc + " ";
            conection = ClassSQL.GetConect();
            conection.Open();
                try
                {
                    sclCom2 = new SqlCommand(sc_getDate, conection);
                    DataSet datset = new DataSet("datetime");
                    SqlDataAdapter dat = new SqlDataAdapter(sclCom2);
                    dat.Fill(datset, "Datetime");
                    foreach (DataRow row in datset.Tables["Datetime"].Rows)
                    {
                        listBox2.Items.Add(row["date_session"]);
                    }

                }

                catch (Exception ex)
                {
                MessageBox.Show("Ошибка системы. ", "Ошибка" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
                finally
                {
                    conection.Close();
                    conection.Dispose();
                }
            listBox2.Visible = true;
            label3.Visible = true;
            Filmc = 0;
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            label4.Visible = true;
            label_cinema.Visible = true;
            label1.Visible = true;
            label_Kolvo.Visible = true;
            button_Vibor.Visible = true;
            try
            {
                LoadKolvo();
                LoadCinema();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка системы. ", "Ошибка" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
            }
        }

        
        string convDat(string oldDate) //КОНВЕРТИРУЕМ ДАТУ 
        {
            string newdate;
            string data = oldDate.Split(' ')[0];
            string vreme = oldDate.Split(' ')[1];
            string[] a1 = new string[3];
            a1 = data.Split('.').ToArray();
            string god = a1[2];
            string mes = a1[1];
            string day = a1[0];

            newdate = god + "-" + mes + "-" + day + " " + vreme;
            return newdate;
        }

        private void LoadKolvo()
        {

            SqlConnection conection = ClassSQL.GetConect();
            SqlConnection conection3 = ClassSQL.GetConect();
            conection.Open();
            conection3.Open();
            try
            {

                string cs_date = "set language english; select session_id from film_session where date_session = '" + convDat(listBox2.SelectedItem.ToString()) + "' ;";

                sclCom5 = new SqlCommand(cs_date, conection);


                go = sclCom5.ExecuteScalar().ToString();

                idha = go;

                string cs_truemesto = "select count(place.place_id) from place where free=0 and session_id=" + go + ";";
                sclCom3 = new SqlCommand(cs_truemesto, conection3);
                string truemesto = sclCom3.ExecuteScalar().ToString();

                label_Kolvo.Text = (Int32.Parse(truemesto)).ToString();
                conection.Close();
                conection.Dispose();
                conection3.Close();
                conection3.Dispose();
            }
            catch
            {
                MessageBox.Show("Ошибка системы. ", "Ошибка" , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void LoadCinema()
        {

            SqlConnection conection = ClassSQL.GetConect();
            SqlConnection conection3 = ClassSQL.GetConect();
            conection.Open();
            conection3.Open();
            string cs_date = "set language english; select session_id from film_session where date_session = '" + convDat(listBox2.SelectedItem.ToString()) + "' ;";

            sclCom5 = new SqlCommand(cs_date, conection);

            go = sclCom5.ExecuteScalar().ToString();//id_session
            idha = go;

            string cs_truecinema = "select c.cinema_name from film_session f join cinema c on c.cinema_id=f.cinema_id and f.session_id=" + go + ";";
            sclCom3 = new SqlCommand(cs_truecinema, conection3);
            string truecinema = sclCom3.ExecuteScalar().ToString(); 

            label_cinema.Text = (truecinema).ToString(); 
            conection.Close();
            conection.Dispose();
            conection3.Close();
            conection3.Dispose();

        }


        private void button_Vibor_Click(object sender, EventArgs e)
        {
            SqlConnection conection = ClassSQL.GetConect();
            conection.Open();
            try
            {
                string cmdText;
                cmdText = "select hall_id from film_session where session_id = " + idha + " "; //id_hall
                sclCom4 = new SqlCommand(cmdText, conection);
                og = sclCom4.ExecuteScalar().ToString();


            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка системы. ", "Ошибка" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            conection.Close();
            conection.Dispose();
            Form Mesto = new Mesto(id_p);
            Mesto.Show();
            this.Hide();
        }

        private void ViborSeans_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
        }


    }
}
