using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewApteka
{
    public partial class Form8 : Form
    {
        public Form8()
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            string command = $"select * from Авторизация where Роль = 'worker'";
            Form1 f = new Form1();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command, f.connection);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds, "Заказ");
            dataGridView1.DataSource = ds.Tables["Заказ"].DefaultView;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form11 ff = new Form11();
            if (ff.ShowDialog() == DialogResult.OK) {
                Add_Client(ff.textBox1.Text, ff.textBox2.Text, ff.comboBox1.Text);
                Form8_Load(sender, e);
            }
        }

        private void Add_Client(string login, string password, string role)
        {
            string command = $"insert into Авторизация (Логин, Пароль, Роль) values ('{login}','{password}','{role}')";
            My_Execute_Non_Query(command);
        }
    }
}
