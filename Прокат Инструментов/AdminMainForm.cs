using System;
using System.Windows.Forms;

namespace Прокат_Инструментов
{
    public partial class AdminMainForm : Form
    {
        public AdminMainForm()
        {
            InitializeComponent();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Customer cus = new Customer();
            cus.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Tools tool = new Tools();
            tool.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Order ord = new Order();
            ord.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Users us = new Users();
            us.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Login log = new Login();
            log.Show();
            this.Hide();
        }
    }
}
