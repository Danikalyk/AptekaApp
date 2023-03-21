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

namespace NewApteka
{
    public partial class Form1 : Form
    {
        public string connection = @"Data Source=DESKTOP-A6KEL66\SQLEXPRESS;Initial Catalog=APTEKA2;User ID=sa;Password=EmmojoyProoo123";
        public int user_id;
        public string user_role;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string SqlCommand = "";

            int row;
            

            SqlCommand = $"select [Код_пользователя], [Роль] from [Авторизация] where [Авторизация].[Логин] = '{textBox1.Text}' and [Авторизация].[Пароль] = '{textBox2.Text}'";
           
                SqlDataAdapter dataAdapter = new SqlDataAdapter(SqlCommand, connection);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds, "[Авторизация]");
            try
            {
                dataGridView1.DataSource = ds.Tables["[Авторизация]"].DefaultView;
                if (dataGridView1[0, 0].Value != null && dataGridView1[1, 0].Value != null)
                {
                    row = dataGridView1.CurrentCell.RowIndex; // взяли строку с dataGridView1
                    user_id = Convert.ToInt32(dataGridView1[0, row].Value);

                    row = dataGridView1.CurrentCell.RowIndex; // взяли строку с dataGridView1
                    user_role = Convert.ToString(dataGridView1[1, row].Value);

                    if (user_role == "worker")
                    {
                        D.user_id = user_id;
                        Form3 f = new Form3();
                        f.Show();
                        this.Hide();
                        textBox1.Clear();
                        textBox2.Clear();
                    }
                    if (user_role == "user")
                    {
                        D.user_id = user_id;
                        Form2 f = new Form2();
                        f.Show();
                        this.Hide();
                        textBox1.Clear();
                        textBox2.Clear();
                    }
                   
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!");
                    textBox2.Clear();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Возникла непредвиденная ошибка, пожалуйста обратитесь к системному администратору!");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
