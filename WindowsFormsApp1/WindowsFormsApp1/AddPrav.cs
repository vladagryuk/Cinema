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
    public partial class AddPrav : Form
    {
        SqlCommand scom3;
        string sc_getPers = "select * from cinema_card;";
        public AddPrav()
        {
            InitializeComponent();

            SqlConnection conection = ClassSQL.GetConect();
            conection.Open();
            try
            {
                Filldgv(conection);
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
        }

        private void Filldgv(SqlConnection con)
        {
            scom3 = new SqlCommand(sc_getPers, con);
            DataSet datso = new DataSet("cinema_card");
            SqlDataAdapter da = new SqlDataAdapter(scom3);
            da.Fill(datso, "cinema_card");
            dataGridView1.DataSource = datso.Tables["cinema_card"];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sc_writepokup = "insert into cinema_card (surname_card, name_card, patronymic_card, phone_pers,points) values('" + textBox1.Text + "', '" + textBox2.Text + "', '" + textBox3.Text + "', '" + textBox4.Text + "', '" + textBox5.Text + "');";
            SqlConnection conection = ClassSQL.GetConect();
            conection.Open();
            try
            {
                scom3 = new SqlCommand(sc_writepokup, conection);
                scom3.ExecuteNonQuery();
                MessageBox.Show("Добавление прошло успешно!", "Уведомление", MessageBoxButtons.OK,MessageBoxIcon.Information);
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                dataGridView1.Refresh();
                SqlConnection conection2 = ClassSQL.GetConect();
                conection2.Open();
                Filldgv(conection2);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка. Возможно данные введены некорректно. ", "Ошибка" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            conection.Close();
            conection.Dispose();


        }
    }
}
