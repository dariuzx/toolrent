using System;
using System.Windows.Forms;

namespace Прокат_Инструментов
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int startpoint = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            startpoint += 1;
            LoadProgress.Value = startpoint;
            if (LoadProgress.Value == 100)
            {
                LoadProgress.Value = 0;
                timer1.Stop();
                Choose cho = new Choose();
                cho.Show();
                this.Hide();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
