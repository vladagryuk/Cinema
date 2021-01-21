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
    public partial class AddUser : Form
    {
        SqlCommand scom3;
        string sc_getPers = "select * from Personal;";
        public AddUser()
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
            DataSet datso = new DataSet("personal");
            SqlDataAdapter da = new SqlDataAdapter(scom3);
            da.Fill(datso, "pers");
            dataGridView1.DataSource = datso.Tables["pers"];

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sc_writepers = "  insert into personal (cinema_id, position, surname_pers,name_pers, patronymic_pers, passport_seria, passport_number, phone_pers, login_pers,password_pers) values ('" + textBox_kin.Text + "', '" + textBox_dolj.Text + "', '" + textBox_fam.Text + "', '" + textBox_nam.Text + "', '" + textBox_otc.Text + "', '" + textBox_pasp.Text + "', '" + textBox_pasp2.Text + "', '" + textBox_mob.Text + "', '" + textBox_log.Text + "', '" + textBox_pass.Text + "');";
            SqlConnection conection = ClassSQL.GetConect();
            conection.Open();




            try
            {
                scom3 = new SqlCommand(sc_writepers, conection);
                scom3.ExecuteNonQuery();
                MessageBox.Show("Добавление прошло успешно!", "Уведомление", MessageBoxButtons.OK,MessageBoxIcon.Information);
                textBox_kin.Clear();
                textBox_pasp.Clear();
                textBox_pasp2.Clear();
                textBox_mob.Clear();
                textBox_nam.Clear();
                textBox_fam.Clear();
                textBox_otc.Clear();
                textBox_log.Clear();
                textBox_pass.Clear();
                dataGridView1.Refresh();
                SqlConnection conection2 = ClassSQL.GetConect();
                conection2.Open();
                Filldgv(conection2);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка. Возможно вы ввели неверные данные. ", "Ошибка" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            conection.Close();
            conection.Dispose();


        }

      
    }
}

