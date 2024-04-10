using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Прокат_Инструментов
{
    public partial class EmpCustomer : Form
    {
        public EmpCustomer()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\dariuz\Documents\ToolsRentaldb.mdf;Integrated Security=True;Connect Timeout=30");
        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void populate()
        {
            Con.Open();
            string query = "select * from CustomerTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            EmpCustDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (CustIdTb.Text == "" || CustNameTb.Text == "" || CustFamTb.Text == "" || CustAddTb.Text == "" || PhoneTb.Text == "")
                    MessageBox.Show("Не во всех полях есть данные");
                else
                {
                    Con.Open();

                    // Проверка, существует ли уже такой клиент
                    string checkQuery = "SELECT COUNT(*) FROM CustomerTbl where Код_клиента =" + CustIdTb.Text + ";";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, Con))
                    {
                        checkCmd.Parameters.AddWithValue("Код_клиента", CustIdTb.Text);

                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Клиент с таким ID уже существует!");
                            Con.Close();
                        }

                        else
                        {
                            Con.Close();
                            Con.Open();
                            string query = "insert into CustomerTbl values(" + CustIdTb.Text + ", N'" + CustNameTb.Text + "', N'" + CustFamTb.Text + "', N'" + CustAddTb.Text + "','" + PhoneTb.Text + "')";
                            SqlCommand cmd = new SqlCommand(query, Con);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Клиент добавлен успешно!");
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

        private void EmpCustDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow selectedRow = EmpCustDGV.Rows[e.RowIndex];
            CustIdTb.Text = selectedRow.Cells[0].Value.ToString();
            CustNameTb.Text = selectedRow.Cells[1].Value.ToString();
            CustFamTb.Text = selectedRow.Cells[2].Value.ToString();
            CustAddTb.Text = selectedRow.Cells[3].Value.ToString();
            PhoneTb.Text = selectedRow.Cells[4].Value.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            EmpMainForm emain = new EmpMainForm();
            emain.Show();
        }
    }
    
}

