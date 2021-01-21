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
using System.Net;
using System.Net.Mail;

namespace WindowsFormsApp1
{
    public partial class Oplata : Form
    {
        SqlCommand sclCom;
        SqlCommand sclCom2;
        SqlCommand sclCom3;
        SqlCommand sclCom4;
        SqlCommand sclCom5;
        SqlCommand sclCom6;


        int id_pok=0;
        int num_card;
        double price;
        double skidka;
        double skidka2;
        string mestechko;
        int ryad;
        bool skid_get = false;

        //для почты
        string kinoteatr,film,date_ses,hall;
 
        public Oplata(string vib_mesto, int price2, int id_p,int num_ryad)
        {
            InitializeComponent();
            ryad = num_ryad;
            price = price2;
            textBox_sum.Text = price.ToString();
            id_pok = id_p; 

            mestechko = vib_mesto;
            if (id_pok == 0) //вывод информации о неизвестном покупателе
            {
                groupBox1.Location = new System.Drawing.Point(1, 240);
                label_Name.Visible = false;
                label_Fam.Visible = false;
                label_Otc.Visible = false;
                label_numcard.Visible = false;
                label_Bal.Visible = false;
                label10.Visible = false;
                label7.Visible = false;
                label9.Visible = false;
                label8.Visible = false;
                label5.Visible = false;
            }
            else if (id_pok == -1) 
            {
                label_Name.Visible = false;
                label_Fam.Visible = false;
                label_Otc.Visible = false;
                label_numcard.Visible = false;
                label_numcard.Text = "admin";
                label_Bal.Text = "999999";
            }
            SqlConnection conection = ClassSQL.GetConect();
            conection.Open();
            try
            {
                GetName(conection);
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

        private void button1_Click(object sender, EventArgs e) 
        {
            if (!maskedTextBox_card.MaskFull)
            {
                MessageBox.Show("Данные введенны некорректно.");
                this.Hide();
            }

            if (!maskedTextBox_srok.MaskFull)
            {
                MessageBox.Show("Данные введенны некорректно. ");
                this.Hide();
            }

            if (!maskedTextBox_cvv.MaskFull)
            {
                MessageBox.Show("Данные введенны некорректно.");
                this.Hide();
            }

            string cmdText;
            string cmdText2;
            string cmdText3;
            SqlConnection conection = ClassSQL.GetConect();
            SqlConnection conectionn = ClassSQL.GetConect();
            SqlConnection conection3 = ClassSQL.GetConect();
            conectionn.Open();
            conection.Open();
            conection3.Open();
            cmdText = "insert into ticket(session_id,place_id,card_id,date_ticket,time_ticket,price,payment) values ('" + ViborSeans.idha + "', '" + mestechko + "', '" + id_pok + "',convert(varchar(10),getdate(),102),convert(varchar(8),getdate(),108)," + price + ",1);";
            cmdText2 = "update place set free = 1 where place = " + mestechko + " and session_id = " + ViborSeans.idha + ";";   
            cmdText3 = "update cinema_card set points = '" + skidka2.ToString() + "' where card_id = " + id_pok.ToString() + ";";
            
            if ((maskedTextBox_card.Text != String.Empty) && (maskedTextBox_srok.Text != String.Empty) && (maskedTextBox_cvv.Text != String.Empty) && (textBox_mail.Text != String.Empty))
            { 
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
                        MessageBox.Show("Ошибка системы. " + ex.Message);
                    }
                    finally
                    {
                        if (id_pok != 0)
                        {
                            try
                            {
                                if (skid_get == false)
                                {
                                    skidka2 += Int32.Parse(textBox_sum.Text) * 0.3;

                                }
                                sclCom6 = new SqlCommand(cmdText3, conection3);
                                sclCom6.ExecuteNonQuery();
                                conection3.Close();
                                conection3.Dispose();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Ошибка системы.  " + ex.Message);
                            }
                        }
                        else
                        {
                            conection.Close();
                            conection.Dispose();
                            conectionn.Close();
                            conectionn.Dispose();
                            MessageBox.Show("Оплата прошла успешно!", "Уведомление", MessageBoxButtons.OK);
                            MessageBox.Show("Ваше купленое место: " + mestechko + " Ряд: " + ryad + "\nНа указанную почту отправлен билет. ", "Уведомление", MessageBoxButtons.OK);


                            
                            string sc_getSes = "select c.cinema_name,CONVERT(varchar(16),date_session,120), f.film_name, h.hall_name from film_session s join cinema c on c.cinema_id=s.cinema_id and session_id="+ ViborSeans.idha +" join film f on f.film_id=s.film_id join hall h on h.hall_id=s.hall_id"; 
                            SqlConnection conection7 = ClassSQL.GetConect();
                            conection7.Open();
                            sclCom3 = new SqlCommand(sc_getSes, conection7);
                            using (DbDataReader reader = sclCom3.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        kinoteatr= reader.GetString(0);
                                        hall = reader.GetString(3);
                                        date_ses = reader.GetString(1);
                                        film = reader.GetString(2);
                                    }

                                }
                            }

                            //@@@ОТПРАВКА ПИСЬМА@@@
                            MailAddress From = new MailAddress("bestkinoteatr@mail.ru", "Кинотеатр");
                            string otpr = textBox_mail.ToString();
                            MailAddress To = new MailAddress(otpr);
                            MailMessage msg = new MailMessage(From, To);
                            msg.Subject = "Ваш билет.";
                            msg.Body = "<h2>Здравствуй, дорогой друг!<h2><br><h3><b>Мы очень ценим то, что вы выбрали наш кинотеатр.<br>Желаем хорошо провести время и приятного просмотра.<b><h3><br><h4><i>Ниже представленна информация по купленному вами билету. Покажите это письмо на кассе.<i><h4><br><h5> Кинотеатр: " + kinoteatr + "<br>Зал: " + hall + "<br>Ряд: " + ryad + "<br>Место: " + mestechko + "<br>Дата: " + date_ses + "<br>Фильм: " + film + "<h5><br><h6><i>С уважением, ваш любимый кинотеатр :)<i><h6>";
                            msg.IsBodyHtml = true;
                            SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
                            smtp.Credentials = new NetworkCredential("bestkinoteatr@mail.ru", "Kino321654");
                            smtp.EnableSsl = true;
                            smtp.Send(msg);

                            this.Hide();
                        }
                    }
             
                    }
                }
            else
            {
                MessageBox.Show("Введите корректные данные. ", "Уведомление", MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            }
        

        private void GetName(SqlConnection con) //информация о покупателе
        {
            string sc_getPokup = "select * from cinema_card where card_id= " + id_pok + " ";
            sclCom = new SqlCommand(sc_getPokup, con);
            using (DbDataReader reader = sclCom.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (id_pok == reader.GetInt32(0))
                        {
                            label_Name.Text = reader.GetString(1);
                            label_Fam.Text = reader.GetString(2);
                            label_Otc.Text = reader.GetString(3);
                            label_numcard.Text = reader.GetInt32(0).ToString();

                            num_card = reader.GetInt32(0);
                            try
                            {
                                LoadCard();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Ошибка системы. " + ex.Message);
                            }
                            finally
                            {
                            }
                        }
                        
                    }

                }
            }

        }

        private void LoadCard()
        {
            string sc_getBal = "select * from cinema_card where card_id=" + num_card.ToString() + " "; 
            SqlConnection conection2 = ClassSQL.GetConect();
            conection2.Open();
            sclCom2 = new SqlCommand(sc_getBal, conection2);
            using (DbDataReader reader = sclCom2.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if ((num_card) == reader.GetInt32(0))
                        {
                            label_Bal.Text = reader.GetValue(5).ToString();
                        }
                    }
                }
            }
            conection2.Close();
            conection2.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            if (id_pok == 0)
            {
                MessageBox.Show("Доступно только для обладателей карты", "Уведомление", MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            else if (skid_get == false)
            {   
                price = price * 0.3; 
                if(Int32.Parse(label_Bal.Text) < price) 
                {
                    MessageBox.Show("На счету недостаточно бонуcов.", "Уведомление", MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
                else
                {
                    skidka = Int32.Parse(textBox_sum.Text) - price;
                    textBox_sum.Text = skidka.ToString();
                    skidka2 = Int32.Parse(label_Bal.Text) - price;
                    label_Bal.Text = skidka2.ToString();
                    skid_get = true;
                    MessageBox.Show("Скидка успешко применена", "Уведомление", MessageBoxButtons.OK,MessageBoxIcon.Information);

                }
            }
            else
            {
                MessageBox.Show("Скидка уже была использована.", "Уведомление", MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        private void Oplata_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
        }


    }
}
