using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace NewApteka
{
    public partial class Form2 : Form
    {
        int count;
        string today_date;
        string zabor_date;
        int user_id;
        public Form2()
        {
            InitializeComponent();
        }

        public void My_Execute_Non_Query(string CommandText)
        {
            Form1 f = new Form1();
            SqlConnection conn = new SqlConnection(f.connection);
            conn.Open();
            SqlCommand myCommand = conn.CreateCommand();
            myCommand.CommandText = CommandText;
            myCommand.ExecuteNonQuery();
            conn.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            user_id = D.user_id;
            label1.Text += Convert.ToString(user_id);

            button2_Click(sender, e);

            numericUpDown1.Maximum = 5;
            numericUpDown1.Minimum = 1;

            Form1 f = new Form1();
            SqlConnection connRC = new SqlConnection(f.connection);
            string command = "select Категории.Категория from Категории";
            SqlDataAdapter da = new SqlDataAdapter(command, connRC);

            DataSet ds = new DataSet();
            connRC.Open();
            da.Fill(ds);
            connRC.Close();

            comboBox1.DataSource = ds.Tables[0];
            comboBox1.DisplayMember = "Категория";

            today_date = Convert.ToString(dateTimePicker1.Value);
        }

        

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2_Click(sender, e);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button2_Click(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string command = $" select Препараты.Код_препарата, Препараты.Наименование, Категории.Категория, Препараты.Цена, Поставщики.Название_организации, Склад.Количество_препаратов from Склад, Поставщики, Препараты, Категории where Препараты.Код_категории = Категории.Код_категории and Препараты.Код_поставщика = Поставщики.Код_поставщика and Препараты.Код_препарата = Склад.Код_препарата and Склад.Количество_препаратов > 0";

            if (!String.IsNullOrWhiteSpace(textBox1.Text))
            {
                command += $" and Препараты.Наименование like '%{textBox1.Text}%'";
            }
            else if (!String.IsNullOrWhiteSpace(comboBox1.Text))
            {
                command += $" and Категории.Категория = '{comboBox1.Text}'";
            }

            Form1 f = new Form1();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command, f.connection);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds, "Препараты");
            dataGridView1.DataSource = ds.Tables["Препараты"].DefaultView;
            dataGridView1.Columns[0].Visible = false;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string path = $"check.txt";

                string check_text;

                int row;
                int id_p;
                int stoim;
                int new_prep_count;

                string prep_name;

                row = dataGridView1.CurrentCell.RowIndex;
                id_p = Convert.ToInt32(dataGridView1[0, row].Value);
                int prep_count = Convert.ToInt32(dataGridView1[5, row].Value);

                count = Convert.ToInt32(numericUpDown1.Value);
                zabor_date = Convert.ToString(dateTimePicker2.Value);

                stoim = Convert.ToInt32(dataGridView1[3, row].Value) * count;

                if (count <= prep_count)
                {
                    prep_name = Convert.ToString(dataGridView1[1, row].Value);
                    new_prep_count = prep_count - count;
                    string command = $"insert into Заказы (Код_клиента, Код_сотрудника, Код_препарата, Дата_приёма, Дата_исполнения, Стоимость_заказа) values ({D.user_id}, 0, {id_p}, '{today_date}', '{zabor_date}', {stoim})";
                    My_Execute_Non_Query(command);
                    string command2 = $"update Склад set Количество_препаратов = {new_prep_count} where Код_препарата = {id_p}";

                    check_text = $"Заказанный товар: {prep_name}\nКоличество: {count}\nЦена: {stoim}\nДата заказа: {today_date}\nДата забора: {zabor_date}";

                    using (StreamWriter writer = new StreamWriter(path, false))
                    {
                        await writer.WriteLineAsync(check_text);
                    }
                    My_Execute_Non_Query(command2);
                    Get_Zakaz();
                    MessageBox.Show("Заказ оформлен");
                }
                else
                {
                    MessageBox.Show("Нужного количества товара нет в наличии!");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Что-то пошло не так!");
            }
        }

        private void Get_Zakaz()
        {
            Form1 f = new Form1();
            string CommandText = $"select Препараты.Код_препарата, Препараты.Наименование, Категории.Категория, Препараты.Цена, Поставщики.Название_организации, Склад.Количество_препаратов from Склад, Поставщики, Препараты, Категории where Препараты.Код_категории = Категории.Код_категории and Препараты.Код_поставщика = Поставщики.Код_поставщика and Препараты.Код_препарата = Склад.Код_препарата and Склад.Количество_препаратов > 0";
            SqlDataAdapter dataAdapter = new SqlDataAdapter(CommandText, f.connection);

            DataSet ds = new DataSet();
            dataAdapter.Fill(ds, "Заказы");
            dataGridView1.DataSource = ds.Tables[0].DefaultView;
            dataGridView1.Columns[0].Visible = false;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
