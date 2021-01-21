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
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using Office = Microsoft.Office.Core;


namespace WindowsFormsApp1
{
    public partial class Otchet_vir : Form
    {
        SqlCommand scom;
        SqlConnection con;
        SqlCommand scom3;

        string itogo;
        string znach = " ";

        public Otchet_vir()
        {
            InitializeComponent();
        }

        private void Filldgv(SqlConnection con)
        {
            string sc_getfilm = "select f.film_name as 'Фильм',count(t.ticket_id) as 'Количество проданных билетов', sum(price) as 'Выручка за период' from ticket t join film_session s on s.session_id=t.session_id and t.payment=1 and t.date_ticket<=dateadd(month," + znach + ",getdate()) and t.date_ticket>=dateadd(month,-" + znach + ",getdate()) join film f on f.film_id=s.film_id group by f.film_name;";



            scom = new SqlCommand(sc_getfilm, con);
            DataSet datso = new DataSet("datso_seanses");
            SqlDataAdapter da = new SqlDataAdapter(scom);
            da.Fill(datso, "seanses");
            dataGridView1.DataSource = datso.Tables["seanses"];

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //получение значения ИТОГО
            con = ClassSQL.GetConect();
            string sc_selectID = "select sum(price) as 'Выручка за период' from ticket t join film_session s on s.session_id = t.session_id and t.payment = 1 and t.date_ticket <= dateadd(month," + znach + ", getdate()) and t.date_ticket >= dateadd(month, -" + znach + ", getdate());";
            con.Open();
            scom3 = new SqlCommand(sc_selectID, con);
            itogo = scom3.ExecuteScalar().ToString();
            con.Close();
            con.Dispose();



            SFD.Filter = "Файлы Excel (*.xls; *.xlsx) | *.xls; *.xlsx";
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
                Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
                app.Visible = true;
                worksheet = workbook.ActiveSheet;
                worksheet.Name = "Выручка";
                worksheet.Cells[1, 1] = "Фильм";
                worksheet.Cells[1, 2] = "Количество проданных билетов";
                worksheet.Cells[1, 3] = "Сумма(в руб.)";
                worksheet.Cells[12, 1] = "Итого:";
                worksheet.Cells[12, 3] = itogo;

                for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
                {
                    worksheet.Cells[2, i] = dataGridView1[i - 1, 0].Value;
                    worksheet.Columns[i].ColumnWidth = 30;
                }
                for (int i = 1; i < dataGridView1.RowCount; i++)
                    for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dataGridView1[j, i].Value;
                    }
                workbook.SaveAs(SFD.FileName, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing,
                Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing);

                app.Quit();

                
            } 
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            znach = comboBox1.SelectedItem.ToString();
            SqlConnection conection = ClassSQL.GetConect();
            conection.Open();

            if (znach != " ")
            {

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
                    //conection.Close();
                    //conection.Dispose();
                    dataGridView1.Refresh();
                }
            }


        }

    }
}
