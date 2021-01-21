using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    public partial class AddSeans : Form
    {
        SqlCommand scom3;
        string sc_getPers = "select c.cinema_name, h.hall_name,f.film_name, s.date_session from film_session s join film f on f.film_id=s.film_id join cinema c on c.cinema_id=s.cinema_id join hall h on h.hall_id=s.hall_id;";
        string sc_getKino = "select cinema_name from  cinema;";
        string sc_getZal = "select hall_name from  hall;";
        string sc_getFilm = "select film_name from  film;";
        SqlConnection con;
        string Cinema;
        string Zal;
        string Film;
        public AddSeans()
        {
            InitializeComponent();
            con = ClassSQL.GetConect();
            con.Open();
            try
            {
                Filldgv(con);
                Fill1com();
                Fill2com();
                Fill3com();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка системы. ", "Ошибка" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
        }

        private void Filldgv(SqlConnection con) 
        {

            scom3 = new SqlCommand(sc_getPers, con);
            DataSet datso = new DataSet("film_session");
            SqlDataAdapter da = new SqlDataAdapter(scom3);
            da.Fill(datso, "film_session");
            dataGridView1.DataSource = datso.Tables["film_session"]; 
        }

        private void Fill1com()
        {
            scom3 = new SqlCommand(sc_getKino, con);
            DataSet ds = new DataSet("cinema_name");
            SqlDataAdapter dat = new SqlDataAdapter(scom3);
            dat.Fill(ds, "cinema_name");
            foreach (DataRow row in ds.Tables["cinema_name"].Rows) 
            {
                comboBox1.Items.Add(row["cinema_name"]);
            }
        }

        private void Fill2com()
        {
            scom3 = new SqlCommand(sc_getZal, con);
            DataSet ds = new DataSet("hall_name");
            SqlDataAdapter dat = new SqlDataAdapter(scom3);
            dat.Fill(ds, "hall_name");
            foreach (DataRow row in ds.Tables["hall_name"].Rows)
            {
                comboBox2.Items.Add(row["hall_name"]);
            }
        }

        private void Fill3com()
        {
            scom3 = new SqlCommand(sc_getFilm, con);
            DataSet ds = new DataSet("film_name");
            SqlDataAdapter dat = new SqlDataAdapter(scom3);
            dat.Fill(ds, "film_name");
            foreach (DataRow row in ds.Tables["film_name"].Rows)
            {
                comboBox3.Items.Add(row["film_name"]);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            con = ClassSQL.GetConect(); 
            string sc_selectID = "select cinema_id from cinema where cinema_name = '" + comboBox1.SelectedItem.ToString() + "';";
            con.Open();
            scom3 = new SqlCommand(sc_selectID, con);
            Cinema = scom3.ExecuteScalar().ToString();
            con.Close();
            con.Dispose();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            con = ClassSQL.GetConect(); 
            string sc_selectID = "select film_id from film where film_name = '" + comboBox3.SelectedItem.ToString() + "';";
            con.Open();
            scom3 = new SqlCommand(sc_selectID, con);
            Film = scom3.ExecuteScalar().ToString();
            con.Close();
            con.Dispose();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            con = ClassSQL.GetConect(); 
            string sc_selectID = "select hall_id from hall where hall_name = '" + comboBox2.SelectedItem.ToString() + "';";
            con.Open();
            scom3 = new SqlCommand(sc_selectID, con);
            Zal = scom3.ExecuteScalar().ToString();
            con.Close();
            con.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            con = ClassSQL.GetConect();
            string sc_insert = "insert into film_session(cinema_id,date_session,film_id,hall_id) values ('" + Cinema + "', '" + convDat(maskedTextBox1.Text) + "', '" + Film + "', '" + Zal + "');";

            SqlConnection conection3 = ClassSQL.GetConect();
            con.Open();
            try
            {
                scom3 = new SqlCommand(sc_insert, con);
                scom3.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Данные введены некорректно. " + ex.Message);
            }
            finally
            {
                con.Close();
                con.Dispose();
                MessageBox.Show("Сеанс добавлен в систему. ", "Уведомление", MessageBoxButtons.OK);
                comboBox1.Text="";
                comboBox2.Text = "";
                comboBox3.Text = "";
                maskedTextBox1.Text = "";
                dataGridView1.Refresh();
                SqlConnection conection2 = ClassSQL.GetConect();
                conection2.Open();
                Filldgv(conection2);
            }

            string convDat(string oldDate)
            {
                MessageBox.Show(oldDate);
                string newdate;
                string data = oldDate.Split(' ')[0];
                string vreme = oldDate.Split(' ')[1];
                string[] a1 = new string[3];
                a1 = data.Split('-').ToArray();
                string god = a1[2];
                string mes = a1[1];
                string day = a1[0];

                newdate = god + "-" + mes + "-" + day + " " + vreme;
                return newdate;
            }
        }
    }
}

