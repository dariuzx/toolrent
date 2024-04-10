using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Прокат_Инструментов
{
    public partial class Users : Form
    {

        public Users()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\dariuz\Documents\ToolsRentaldb.mdf;Integrated Security=True;Connect Timeout=30");
        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
       // отображение пользователей в таблице Users.cs 
        private void populate()
        {
            Con.Open();
            string query = "select * from UserTbl";
            SqlDataAdapter da = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(da);
            var ds = new DataSet();
            da.Fill(ds);
            UserDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        // внесение ланных пользователя users.cs
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (UidTb.Text == "" || UpassTb.Text == "" || UnameTb.Text == "" || UrightsTb.Text == "")
                    MessageBox.Show("Не во всех полях есть данные");
                else
                {
                    Con.Open();

                    // Проверка, существует ли уже такой пользователь
                    string checkQuery = "SELECT COUNT(*) FROM UserTbl where Логин='" + UidTb.Text + "';";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, Con))
                    {
                        checkCmd.Parameters.AddWithValue("Логин", UidTb.Text);

                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Сотрудник с таким ID уже существует!");
                            Con.Close();
                        }

                        else
                        {
                            Con.Close();
                            Con.Open();
                            string query = "insert into UserTbl values('" + UidTb.Text + "', '" + UpassTb.Text + "', N'" + UnameTb.Text + "', N'" + UrightsTb.Text + "')";
                            SqlCommand cmd = new SqlCommand(query, Con);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Сотрудник добавлен!");
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

        private void Users_Load(object sender, EventArgs e)
        {
            populate(); 
        }

        // удаление пользователя из таблицы Users.cs
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (UidTb.Text == "")
                {
                    MessageBox.Show("Выберите сотрудника для удаления");
                }
                else
                {
                    Con.Open();
                    string query = "delete from UserTbl where Логин='" + UidTb.Text + "'";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Сотрудник удален!");
                    Con.Close();
                    populate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UserDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow selectedRow = UserDGV.Rows[e.RowIndex];
            UidTb.Text = selectedRow.Cells[0].Value.ToString();
            UpassTb.Text = selectedRow.Cells[1].Value.ToString();
            UnameTb.Text = selectedRow.Cells[2].Value.ToString();
            UrightsTb.Text = selectedRow.Cells[3].Value.ToString();
        }
        // изменение пользователя в таблице Users.cs
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (UidTb.Text == "" || UpassTb.Text == "" || UnameTb.Text == "" || UrightsTb.Text == "")
                    MessageBox.Show("Не во всех полях есть данные");
                else
                {
                    Con.Open();
                    string query = "update UserTbl set Пароль='" + UpassTb.Text + "', Имя= N'" + UnameTb.Text + "', Права = N'" + UrightsTb.Text + "' WHERE Логин = '" + UidTb.Text + "';";

                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Сотрудник обновлён!");
                    Con.Close();
                    populate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminMainForm main = new AdminMainForm();
            main.Show();
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}       
