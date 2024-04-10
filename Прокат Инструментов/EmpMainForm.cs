using System;
using System.Windows.Forms;

namespace Прокат_Инструментов
{
    public partial class EmpMainForm : Form
    {
        public EmpMainForm()
        {
            InitializeComponent();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            EmpOrder empo = new EmpOrder();
            empo.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login log = new Login();
            log.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            empTools empt = new empTools();
            empt.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            EmpCustomer empc = new EmpCustomer();
            empc.Show();
        }
    }
}
