using System;
using System.Windows.Forms;

namespace Прокат_Инструментов
{
    public partial class Choose : Form
    {
        public Choose()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Login log = new Login();
            log.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GuestTools guest = new GuestTools();
            guest.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }
    }
}
