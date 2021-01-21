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
    public partial class BeginFormAuto : Form
    {
        SqlCommand scomand; 
        string sc_getPers = "select * from personal";
        int id_pers;
        string id_dolj; 



        public BeginFormAuto()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conect = ClassSQL.GetConect();
            conect.Open();
            try
            {
                EnterToSystem(conect); 
            }
            catch
            {
                MessageBox.Show("Ошибка системы. ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conect.Close();
                conect.Dispose();
            }
        }

        private void EnterToSystem(SqlConnection con)
        {
            bool yap = false; 
            string Login = textBoxLogin.Text;
            string Parol = textBoxPassword.Text;
            scomand = new SqlCommand(sc_getPers, con);
            using (DbDataReader reader = scomand.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (Login == reader.GetString(9) && Parol == reader.GetString(10))
                        {
                            yap = true;
                            id_dolj = reader.GetString(2);
                            id_pers = reader.GetInt32(0);
                        }

                    }
                    if (yap == true)
                    {
                        reader.Close();
                            Form main = new PersFrom(id_pers, id_dolj);
                            main.Show();
                            this.Hide();
                        yap = false;
                    }
                    else
                    {
                        MessageBox.Show("Введен неверный логин или пароль.", "Ошибка", MessageBoxButtons.OK,MessageBoxIcon.Error);
                        reader.Close();
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) 
        {
            Form Pokupatel = new PokupatelAuto();
            Pokupatel.Show();
            this.Hide();
        }
    }
}
