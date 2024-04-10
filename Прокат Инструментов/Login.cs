using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Прокат_Инструментов
{
    public partial class Login : Form
    {
        
        public Login()
        {
            InitializeComponent();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\dariuz\Documents\ToolsRentaldb.mdf;Integrated Security=True;Connect Timeout=30");
        DataBase database = new DataBase();

        private void button1_Click(object sender, EventArgs e)
        {
            var loginuser = textBox1.Text;
            var passuser = textBox2.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            string qwerysting = $"select Логин Пароль from UserTbl where Логин = '{loginuser}' and Пароль = '{passuser}'";
            SqlCommand command = new SqlCommand(qwerysting, Con);
            adapter.SelectCommand = command;
            adapter.Fill(table);


            if (table.Rows.Count == 1)
            {

                SqlDataAdapter adapter1 = new SqlDataAdapter();
                DataTable table1 = new DataTable();

                string querystring1 = $"SELECT Логин, Пароль, Имя, Права FROM UserTbl WHERE Логин = '{loginuser}' AND Пароль = '{passuser}' AND Имя = 'adm' AND Права = 'adm'";

                var Login = loginuser;
                var password = passuser;
                SqlCommand command1 = new SqlCommand(querystring1, Con);

                adapter1.SelectCommand = command1;
                adapter1.Fill(table1);

                if (table1.Rows.Count == 1)
                {
                    MessageBox.Show("Вы успешно вошли как админ!", "Вход выполнен!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Hide();
                    Form MainForm = new AdminMainForm();
                    MainForm.Show();
                    textBox1.Text = "";
                    textBox2.Text = "";
                }
                else
                {
                    MessageBox.Show("Вы успешно вошли как сотрудник!", "Вход выполнен!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Hide();
                    Form employee_product = new EmpMainForm();
                    employee_product.Show();
                    textBox1.Text = "";
                    textBox2.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Такого аккаунта не существует!", "Аккаунта не существует!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {
            Choose cho = new Choose();
            cho.Show();
            this.Hide();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*';
            pictureBox2.Visible = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = false;
            pictureBox2.Visible = false;
            pictureBox3.Visible = true;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = true;
            pictureBox2.Visible = true;
            pictureBox3.Visible = false;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }
    }
}
