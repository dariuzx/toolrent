using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Прокат_Инструментов
{
    public partial class GuestTools : Form
    {
        public GuestTools()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\dariuz\Documents\ToolsRentaldb.mdf;Integrated Security=True;Connect Timeout=30");
        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void GuestTools_Load(object sender, EventArgs e)
        {
            populate();
        }
        private void populate()
        {
            Con.Open();
            string query = "select * from ToolsTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            empToolsDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Choose cho = new Choose();
            cho.Show();
            this.Hide();
        }

        private void SearchBox2_TextChanged(object sender, EventArgs e)
        {
            Search2(empToolsDGV);
        }
        private void Search2(DataGridView dgw2)
        {
            Con.Open();
            string searchString = $"select * from ToolsTbl where concat(Код_инструмента, Название_инструмента, Бренд, Цена, Количество) like N'%" + SearchBox2.Text + "%' ";
            SqlCommand com = new SqlCommand(searchString, Con);
            SqlDataReader read = com.ExecuteReader();
            // Устанавливаем источник данных равным null перед выполнением нового запроса
            dgw2.DataSource = null;
            // Очищаем строки
            dgw2.Rows.Clear();
            // Перезагружаем данные из SqlDataReader
            while (read.Read())
            {
                ReadSingleRow2(dgw2, read);
            }
            Con.Close();
        }
        private void ReadSingleRow2(DataGridView dgw2, SqlDataReader reader)
        {
            // Убедитесь, что столбцы созданы (может быть, это было сделано в другом месте кода)
            if (dgw2.Columns.Count == 0)
            {
                // Если столбцы не определены, добавьте их
                dgw2.Columns.Add("Код_инструмента", "Код инструмента");
                dgw2.Columns.Add("Название_инструмента", "Название инструмента");
                dgw2.Columns.Add("Бренд", "Бренд");
                dgw2.Columns.Add("Цена", "Цена");
                dgw2.Columns.Add("Количество", "Количество");
            }
            // Создайте массив значений для строк
            object[] values = new object[reader.FieldCount];
            reader.GetValues(values);
            // Добавьте строку в DataGridView
            dgw2.Rows.Add(values);

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            GuestTools to = new GuestTools();
            to.Show();
        }
    }
}
