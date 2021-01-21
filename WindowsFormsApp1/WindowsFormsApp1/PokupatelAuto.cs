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
    public partial class PokupatelAuto : Form
    {
        string sc_getCard = "select * from cinema_card;";
        int Num_card;
        int Balance;
        SqlCommand scommand;


        public PokupatelAuto()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection connect = ClassSQL.GetConect();
            connect.Open();
            if (textBox_Card.Text != "")
            {
                EnterToSystem(connect);
            }
            else
            {
                MessageBox.Show("Данной карты не существует. Попробуйте снова. ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnterToSystem(SqlConnection trycon)
        {
            bool CardOk = false;
            int LogCard = Int32.Parse(textBox_Card.Text);
            scommand = new SqlCommand(sc_getCard, trycon);
            using (DbDataReader reader = scommand.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (LogCard == reader.GetInt32(0))
                        {
                            CardOk = true;
                            Num_card = reader.GetInt32(0); 
                            Balance = reader.GetInt32(5);
                        }

                    }
                    if (CardOk == true)
                    {
                        Form FormPokup = new PokupForm(Num_card, Balance);
                        FormPokup.Show();
                        this.Hide();
                        CardOk = false;
                    }
                    else
                    {
                        if (CardOk == false)
                        {
                            MessageBox.Show("Номер карты отстутсвует в списке. Попробуйте ввести другие данные.", "Ошибка", MessageBoxButtons.OK,MessageBoxIcon.Error);
                        }

                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
                Form FormPokup = new PokupForm(0, 0 ); 
                FormPokup.Show();
                this.Hide();
        }

        

        private void PokupatelAuto_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
