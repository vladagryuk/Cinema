using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Common;


namespace WindowsFormsApp1
{
    public partial class PokupForm : Form
    {
        SqlCommand scom;
        SqlCommand scom2;
        SqlCommand scom3;
        SqlCommand scom4;
        SqlCommand scom6;
        string sc_getCinema = "select * from cinema";
        string sc_getName = "select * from cinema_card"; 
        string sc_getFilm = "select film_name as ФИЛЬМЫ from film";

        int id_c; // номер карты
        int id_b; //баланс карты
        public static int id_pokupatel = 0; 
        string pathimg;
        string filminf;
        public PokupForm(int Num_card, int Balance)
        {
            InitializeComponent();
            id_c = Num_card;
            id_b = Balance;


            SqlConnection conection = ClassSQL.GetConect();
            SqlConnection conection2 = ClassSQL.GetConect();
            SqlConnection conection3 = ClassSQL.GetConect();
            conection.Open();
            conection2.Open();
            conection3.Open();
            try
            {
                if (Num_card != 0)
                {
                    FullName(conection2);
                }
                    EnterToSys(conection);
                    LoadSeans(conection3);
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

        private void EnterToSys(SqlConnection con)
        {
            scom = new SqlCommand(sc_getCinema, con);
            using (DbDataReader reader = scom.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if(id_c == 0)
                        {
                            label2.Visible = false;
                            label7.Visible = false;
                            label3.Visible = false;
                            label_Name.Visible = false;
                            label_phone.Visible = false;
                            label_Balance.Visible = false;
                            panel2.Location = new Point(23, 45);

                        }
                        else if (id_c != 0)
                        {
                            label_Balance.Text = id_b.ToString();
                        }
                    }
                }
            }
        }
        private void FullName(SqlConnection conn) //вывод информации о клиенте
        {
            scom2 = new SqlCommand(sc_getName, conn);
            using (DbDataReader reader = scom2.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (id_c == reader.GetInt32(0)) 
                        {
                            id_pokupatel = reader.GetInt32(0);
                            label_Name.Text = reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3);
                            label_phone.Text = reader.GetString(4); 
                            label_Balance.Text = id_b.ToString() ; 
                        }
                    }
                }
            }
        }

        private void LoadSeans(SqlConnection connn) //отвечает за таблицы с фильмами
        {
                
              scom3 = new SqlCommand(sc_getFilm, connn);
                DataSet datso = new DataSet("datso_seanses");
                SqlDataAdapter da = new SqlDataAdapter(scom3);
                da.Fill(datso, "Films");
                dataGridView1.DataSource = datso.Tables["Films"];

        }


        private void button1_Click(object sender, EventArgs e)
        {
            Form BiletBuyForm = new ViborSeans(id_pokupatel);
            BiletBuyForm.Show();
            //this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) 
        {
            SqlConnection conection5 = ClassSQL.GetConect();
            conection5.Open();
            
                    if (e.RowIndex != -1 && dataGridView1.CurrentCell != null)
                    {
                      string numFilm = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                      filminf = numFilm;
                      FullNameFilm(conection5);
                     }     
                     else  
                    {
                        MessageBox.Show("Выберите фильм для просмотра информации о нем.");
                    }

        }

        private void FullNameFilm(SqlConnection conn) 
        {
            SqlConnection conection4 = ClassSQL.GetConect();
            conection4.Open();
            string sc_getfilm = "Select f.film_name, f.producer, g.genre_name, f.film_time, f.release_date,f.poster, f.plot  from film f join genre g on g.genre_id = f.genre_id where film_name = '" + filminf + "' ";
            scom4 = new SqlCommand(sc_getfilm, conn);
            using (DbDataReader reader = scom4.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        panel1.Visible = true;
                        label_namefilm.Text = reader.GetString(0);
                        label_producer.Text = reader.GetString(1);
                        label_genre.Text = reader.GetString(2);
                        label_time.Text = reader.GetString(3);
                        label_plot.Text = reader.GetString(6);
                        SqlConnection conection6 = ClassSQL.GetConect();
                        conection6.Open(); 
                        string sc_getimg = "Select poster from film where film_name='"+ filminf + "'"; 
                        scom6 = new SqlCommand(sc_getimg, conection6); 
                        pathimg = scom6.ExecuteScalar().ToString(); 
                        pictureBox1.Image = Image.FromFile(pathimg); 
                        pathimg = scom6.ExecuteScalar().ToString(); 
                        pictureBox1.Image = Image.FromFile(pathimg);
                    }
                }
            }
        } 

        private void PokupForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form Spravka = new Spravka();
            Spravka.Show();
        }

        private void завершитьРаботуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void поменятьПользователяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form BeginFormAuto = new BeginFormAuto();
            BeginFormAuto.Show();
            this.Hide();
        }


    }
}
