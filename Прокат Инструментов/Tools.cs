using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace Прокат_Инструментов
{
    public partial class Tools : Form
    {
        public Tools()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\dariuz\Documents\ToolsRentaldb.mdf;Integrated Security=True;Connect Timeout=30");
        private void populate()
        {
            Con.Open();
            string query = "select * from ToolsTbl";
            SqlDataAdapter da = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(da);
            var ds = new DataSet();
            da.Fill(ds);
            ToolsDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        // внесение инструментов в таблицу Tools.cs
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (RegNumTb.Text == "" || ToolNameTb.Text == "" || BrandTb.Text == "" || PriceTb.Text == "" || QuantityTb.Text == "")
                {
                    MessageBox.Show("Не во всех полях есть данные");
                }

                else
                {
                    Con.Open();

                    // Проверка, существует ли уже такой продукт
                    string checkQuery = "SELECT COUNT(*) FROM ToolsTbl where Код_инструмента=" + RegNumTb.Text + ";";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, Con))
                    {
                        checkCmd.Parameters.AddWithValue("Код_инструмента", RegNumTb.Text);

                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Инструмент с таким ID уже существует!");
                            Con.Close();
                        }

                        else
                        {
                            Con.Close();
                            Con.Open();
                            // Выбор изображения с компьютера
                            OpenFileDialog openFileDialog = new OpenFileDialog();
                            if (openFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                // Получение пути к выбранному файлу
                                string imagePath = openFileDialog.FileName;
                                // Преобразование изображения в массив байтов
                                byte[] imageBytes = File.ReadAllBytes(imagePath);
                                string query = "INSERT INTO ToolsTbl VALUES(@Код_инструмента, @Название_инструмента, @Бренд, @Цена, @Количество, @Изображение)"; 
                                SqlCommand cmd = new SqlCommand(query, Con);
                                // Добавление параметров
                                cmd.Parameters.AddWithValue("@Код_инструмента", RegNumTb.Text); 
                                cmd.Parameters.AddWithValue("@Название_инструмента", ToolNameTb.Text);
                                cmd.Parameters.AddWithValue("@Бренд", BrandTb.Text); 
                                cmd.Parameters.AddWithValue("@Цена", PriceTb.Text);
                                cmd.Parameters.AddWithValue("@Количество", QuantityTb.Text);
                                cmd.Parameters.AddWithValue("@Изображение", imageBytes);
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Инструмент добавлен успешно!");
                                Con.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Tools_Load(object sender, EventArgs e)
        {
            populate();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        // удаление инструментов
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (RegNumTb.Text == "")
                {
                    MessageBox.Show("Выберите инструмент для удаления");
                }
                else
                {
                    Con.Open();
                    string query = "delete from ToolsTbl where Код_инструмента=" + RegNumTb.Text + "";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Инструмент удален!");
                    Con.Close();
                    populate();
                }
            }
            catch (Exception Myex)
            {
                MessageBox.Show(Myex.Message);
            }
        }

        private void ToolsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow selectedRow = ToolsDGV.Rows[e.RowIndex];
            RegNumTb.Text = selectedRow.Cells[0].Value.ToString();
            ToolNameTb.Text = selectedRow.Cells[1].Value.ToString();
            BrandTb.Text = selectedRow.Cells[2].Value.ToString();
            PriceTb.Text = selectedRow.Cells[3].Value.ToString();
            QuantityTb.Text = selectedRow.Cells[4].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (RegNumTb.Text == "" || ToolNameTb.Text == "" || BrandTb.Text == "" || PriceTb.Text == "" || QuantityTb.Text == "")
            {
                MessageBox.Show("Недостающая информация");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = "update ToolsTbl set Название_инструмента= N'" + ToolNameTb.Text + "', Бренд= N'" + BrandTb.Text + "', Цена= '" + PriceTb.Text+"', Количество = '" + QuantityTb.Text + "' where Код_инструмента=" + RegNumTb.Text + "";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Инструмент успешно изменен");
                    Con.Close();
                    populate(); 
                }
                catch (Exception Myex)
                {
                    MessageBox.Show(Myex.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminMainForm main = new AdminMainForm();
            main.Show();
        }

        private void SearchBox2_TextChanged(object sender, EventArgs e)
        {
            Search2(ToolsDGV);
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
            Tools tool = new Tools();
            tool.Show();
        }
    }
}

