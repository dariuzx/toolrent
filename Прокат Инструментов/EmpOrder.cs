using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using QRCoder;

namespace Прокат_Инструментов
{
    public partial class EmpOrder : Form
    {
        public EmpOrder()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\dariuz\Documents\ToolsRentaldb.mdf;Integrated Security=True;Connect Timeout=30");
        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (OrderIdTb.Text == "" || CustomerIdTb.Text == "" || ToolidTb.Text == "" || dateTimePicker1.Text == "" || QuantityTb.Text == "")
                    MessageBox.Show("Не во всех полях есть данные");
                else
                {
                    Con.Open();

                    // Проверка, существует ли уже такой продукт
                    string checkQuery = "SELECT COUNT(*) FROM OrderTbl where Код_заказа =" + OrderIdTb.Text + ";";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, Con))
                    {
                        checkCmd.Parameters.AddWithValue("Код_заказа", OrderIdTb.Text);

                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Заказ с таким ID уже существует!");
                            Con.Close();
                        }

                        else
                        {
                            Con.Close();
                            Con.Open();
                            string formattedDate = dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss");

                            string query = "INSERT INTO OrderTbl VALUES(" + OrderIdTb.Text + ", '" + CustomerIdTb.Text + "', '" + ToolidTb.Text + "', '" + formattedDate + "','" + QuantityTb.Text + "')";
                            SqlCommand cmd = new SqlCommand(query, Con);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Заказ добавлен успешно!");
                            Con.Close();
                            populate();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void populate()
        {
            Con.Open();
            string query = "select * from OrderTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();

            sda.Fill(ds);
            EmpOrderDGV.DataSource = ds.Tables[0];
            Con.Close();


            Con.Open();
            query = "select * from ToolsTbl";
            sda = new SqlDataAdapter(query, Con);
            builder = new SqlCommandBuilder(sda);
            ds = new DataSet();
            sda.Fill(ds);
            EmpToolDGV.DataSource = ds.Tables[0];
            Con.Close();

          
            Con.Open();
            query = "select * from CustomerTbl";
            sda = new SqlDataAdapter(query, Con);
            builder = new SqlCommandBuilder(sda);
            ds = new DataSet();
            sda.Fill(ds);
            EmpCustDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void EmpOrder_Load(object sender, EventArgs e)
        {
            populate();
        }

 

        private void button3_Click(object sender, EventArgs e)
        {
            OrderIdTb.Text = "";
            CustomerIdTb.Text = "";
            ToolidTb.Text = "";
            QuantityTb.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Con.Open();
                        
            string query = $"SELECT * FROM CustomerTbl WHERE Код_клиента = "+CustomerIdTb.Text+"";

            SqlCommand cmd = new SqlCommand(query, Con);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                // Присваиваем значения из DataTable в Label
                lbF.Text = reader["Имя_клиента"].ToString();
                lbI.Text = reader["Фамилия_клиента"].ToString();


                // Выводим значения lbF.Text и lbI.Text для отладки

            }
            
            Con.Close();
            Con.Open();
            


            query = $"SELECT * FROM ToolsTbl WHERE Код_инструмента = "+ToolidTb.Text+"";

            cmd = new SqlCommand(query, Con);
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {

                lbCost.Text = reader["Цена"].ToString();
                lbName.Text = reader["Название_инструмента"].ToString();


                // Выводим значения lbF.Text и lbI.Text для отладки

            }
            
                lbquantiti.Text = QuantityTb.Text;
                // Ваш дальнейший код
            
           

            if (double.TryParse(lbCost.Text, out double value1) && double.TryParse(lbquantiti.Text, out double value2))
            {
                // Выполняем умножение
                double result = value1 * value2;

                // Выводим результат в другой Label
                lbResult.Text = result.ToString();
            }


            //MessageBox.Show($"количество - { lbCost.Text}");
            Con.Close();




            Con.Open();
            query = $"SELECT * FROM CustomerTbl WHERE Имя_клиента = '{lbF.Text}' and Фамилия_клиента = '{lbI.Text}'";

            cmd = new SqlCommand(query, Con);
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                // Присваиваем значения из DataTable в Label
                lbF.Text = reader["Имя_клиента"].ToString();
                lbI.Text = reader["Фамилия_клиента"].ToString();
                lbO.Text = reader["Номер_телефона"].ToString();
            }
            Con.Close();
            string Фио = $"{lbF.Text} {lbI.Text}";


            string txtQrCode = "Прокат инструментов" + "\nЧек покупки товара" + $"\nКлиент: {lbF.Text} {lbI.Text}" + "\nНомер заказа: " + OrderIdTb.Text + "\nНазвание товара: " + lbName.Text +
    "\nКоличество: " + lbquantiti.Text + "\nДата: " + dateTimePicker1.Text + $"\nСтоимость: {lbResult.Text}";



            QRCodeGenerator qr = new QRCodeGenerator();
            QRCodeData data = qr.CreateQrCode(txtQrCode, QRCodeGenerator.ECCLevel.Q);
            QRCode code = new QRCode(data);
            picQR.Image = code.GetGraphic(5);

            if (OrderIdTb.Text == "" || CustomerIdTb.Text == "")
            {
                MessageBox.Show("Не выбран заказ");
            }
            else
            {
                printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
                printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom", 415, 500);
                // Привязка PrintDocument к PrintPreviewDialog
                printPreviewDialog1.Document = printDocument1;

                // Отображение предварительного просмотра
                printPreviewDialog1.ShowDialog();


            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            string Фио = $"{lbF.Text} {lbI.Text} {lbO.Text}";
            e.Graphics.DrawString("Прокат инструментов", new Font("Century Gothic", 12, FontStyle.Bold), Brushes.Black, new Point(70));
            e.Graphics.DrawString("Чек покупки товара:", new Font("Century Gothic", 10, FontStyle.Bold), Brushes.Black, new Point(10, 25));
            e.Graphics.DrawString("Клиент: " + lbF.Text + " " + lbI.Text, new Font("Century Gothic", 10, FontStyle.Bold), Brushes.Black, new Point(10, 50));
            e.Graphics.DrawString("Номер заказа: " + OrderIdTb.Text, new Font("Century Gothic", 10, FontStyle.Bold), Brushes.Black, new Point(10, 70));
            e.Graphics.DrawString("Название товара: " + lbName.Text, new Font("Century Gothic", 10, FontStyle.Bold), Brushes.Black, new Point(10, 90));
            e.Graphics.DrawString("Количество: " + lbquantiti.Text, new Font("Century Gothic", 10, FontStyle.Bold), Brushes.Black, new Point(10, 110));
            //e.Graphics.DrawString("Продавец: " + Фио + "\nКод продовца: " + zakaziList.SelectedRows[0].Cells[1].Value.ToString(), new Font("Century Gothic", 10, FontStyle.Bold), Brushes.Black, new Point(10, 110));
            e.Graphics.DrawString("Дата: " + dateTimePicker1.Text, new Font("Century Gothic", 10, FontStyle.Bold), Brushes.Black, new Point(10, 150));
            e.Graphics.DrawString("Стоимость: " + lbResult.Text + " руб.", new Font("Century Gothic", 10, FontStyle.Bold), Brushes.Black, new Point(10, 170));
            int newWidth = picQR.Width / 2;
            int newHeight = picQR.Height / 2;
            e.Graphics.DrawImage(picQR.Image, new Rectangle(80, 207, 250, 250));
        }

        private void EmpOrderDGV_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow selectedRow = EmpOrderDGV.Rows[e.RowIndex]; 
            OrderIdTb.Text = selectedRow.Cells[0].Value.ToString();
            CustomerIdTb.Text = selectedRow.Cells[1].Value.ToString(); 
            ToolidTb.Text = selectedRow.Cells[2].Value.ToString();
            dateTimePicker1.Text = selectedRow.Cells[3].Value.ToString(); 
            QuantityTb.Text = selectedRow.Cells[4].Value.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
           EmpMainForm emain = new EmpMainForm();
            emain.Show();
        }
    }
}
