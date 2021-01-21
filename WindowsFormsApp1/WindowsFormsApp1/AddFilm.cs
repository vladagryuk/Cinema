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
    public partial class AddFilm : Form
    {
        SqlCommand scom;
        SqlCommand scom2;
        SqlCommand scom3;
        string sc_getfilm = "select * from Film;";
        string sc_getjanr = "select genre_name from genre order by genre_id asc;";
        int janr = 1;

        public AddFilm()
        {
            InitializeComponent();
           
            SqlConnection conection = ClassSQL.GetConect();
            SqlConnection conection2 = ClassSQL.GetConect();
            SqlConnection conection3 = ClassSQL.GetConect();
            conection.Open();
            conection2.Open();
            conection3.Open();
            try
            {
                Filldgv(conection);
                Filljanr(conection2);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка системы. ", "Ошибка" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conection.Close();
                conection.Dispose();
                conection2.Close();
                conection2.Dispose();
                conection3.Close();
                conection3.Dispose();
            }
        }
        private void Filldgv (SqlConnection con)
        {
            scom3 = new SqlCommand(sc_getfilm, con);
            DataSet datso = new DataSet("datso_seanses");
            SqlDataAdapter da = new SqlDataAdapter(scom3);
            da.Fill(datso, "seanses");
            dataGridView1.DataSource = datso.Tables["seanses"];

        }

        string convDat(string oldDate)
        {
            string[] a1 = new string[3];
            a1 = oldDate.Split(',').ToArray();
            string god = a1[2];
            string mes = a1[1];
            string day = a1[0];

            string newdate = day  + "." + mes + "." + god;
            return newdate;
        }

        private void Filljanr(SqlConnection conn)
        {
            scom2 = new SqlCommand(sc_getjanr, conn);
            DataSet ds = new DataSet("film_name");
            SqlDataAdapter dat = new SqlDataAdapter(scom2);
            dat.Fill(ds, "film");
            foreach (DataRow row in ds.Tables["film"].Rows)
            {
                comboBox1.Items.Add(row["genre_name"]);
            }

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e) //что это
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }

        private void Обновить_Click(object sender, EventArgs e) //удалить
        {
            dataGridView1.Refresh();
  
        }
            private void button1_Click(object sender, EventArgs e)
            {
                janr += comboBox1.SelectedIndex; 
                string sc_writefilm = "insert into film(film_name, producer, genre_id, film_time, release_date, end_of_premiere, start_price, poster, plot) values ('" + textBox1.Text + "', '"+ textBox4.Text+ "', '" + janr.ToString() + "', '" + textBox2.Text + "', '" + convDat(maskedTextBox1.Text)+ "', '" + convDat(maskedTextBox2.Text) + "', '" + textBox5.Text.ToString() + "', '" + textBox6.Text + "', '" + textBox7.Text +  "');";
                SqlConnection conection = ClassSQL.GetConect();
                MessageBox.Show(maskedTextBox1.Text);
                conection.Open();
                try
                {
                scom = new SqlCommand(sc_writefilm, conection);
                scom.ExecuteNonQuery();
                MessageBox.Show("Добавление прошло успешно!", "Уведомление", MessageBoxButtons.OK,MessageBoxIcon.Information);
                SqlConnection conection2 = ClassSQL.GetConect();
                conection2.Open();
                Filldgv(conection2); 
            }
                 catch (Exception ex)
                {
                MessageBox.Show("Ошибка системы. ", "Ошибка" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
                conection.Close();
                conection.Dispose();


            }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e) 
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form ersFrom = new PersFrom(PersFrom.id_p, PersFrom.id_d);
            ersFrom.Show();
            this.Hide();
        }
    }
}
