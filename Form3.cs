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
    public partial class Form3 : Form
    {
        public int user_id;
        string path = "check2.txt";
        string check_text;

        //1 - Препараты; 2 - клиенты; 3 - сотрудники; 4 - категории; 5 - поставщики;
        int act_table;
        public Form3()
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

        private void Form3_Load(object sender, EventArgs e)
        {
            Zakaz();
            user_id = D.user_id;
            label1.Text += Convert.ToString(user_id);
            button1_Click(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            act_table = 1;
            Get_Preparate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            act_table = 2;
            Get_Client();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            act_table = 3;
            Get_Worker();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            act_table = 4;
            Get_Category();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            act_table = 5;
            Get_Post();
        }

        private async void Add_Zakaz(int id_c, int id_p, string today_date, string zabor_date, int id_workwer, int stoim, int new_prep_count, string prep_name, int count)
        {
            string command = $"insert into Заказы (Код_клиента, Код_сотрудника, Код_препарата, Дата_приёма, Дата_исполнения, Стоимость_заказа) values ({id_c}, {D.user_id}, {id_p}, '{today_date}', '{zabor_date}', {stoim})";
            My_Execute_Non_Query(command);
            string command2 = $"update Склад set Количество_препаратов = {new_prep_count} where Код_препарата = {id_p}";

            check_text = $"Заказанный товар: {prep_name}\nКоличество: {count}\nЦена: {stoim}\nДата заказа: {today_date}\nДата забора: {zabor_date}";

            using (StreamWriter writer = new StreamWriter(path, false))
            {
                await writer.WriteLineAsync(check_text);
            }
            My_Execute_Non_Query(command2);
            MessageBox.Show("Заказ оформлен");
        }

        private void Zakaz()
        {
            string command = $"select Клиенты.Фамилия as [Фамилия клиента], Клиенты.Имя as [Имя клиента], Сотрудники.Фамилия as [Фамилия сотрудника], Сотрудники.Имя as [Имя сотрудника], Препараты.Наименование, Заказы.Дата_приёма, Заказы.Дата_исполнения, Заказы.Стоимость_заказа from Заказы, Препараты, Клиенты, Сотрудники where Заказы.Код_препарата = Препараты.Код_препарата and Заказы.Код_клиента = Клиенты.Код_клиента and Заказы.Код_сотрудника = Сотрудники.Код_сотрудника";

            if (!String.IsNullOrWhiteSpace(textBox1.Text))
            {
                command += $" and Клиенты.Фамилия like '%{textBox1.Text}%'";
            }
            else if (!String.IsNullOrWhiteSpace(textBox2.Text))
            {
                command += $" and Препараты.Наименование = '%{textBox2.Text}%'";
            }

            Form1 f = new Form1();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command, f.connection);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds, "Заказ");
            dataGridView1.DataSource = ds.Tables["Заказ"].DefaultView;
        }

        private void Add_Preparate(int id_prep, string name, int count, int category, int stoim, int ed_izm, string date_today, string date_izg, string date_god, int post)
        {
            string command = $"insert into Препараты (Код_препарата, Наименование, Код_категории, Цена, Единица_измерения, Дата_производства, Срок_годности, Код_поставщика) values ({id_prep}, '{name}', {category}, {stoim}, {ed_izm}, '{date_izg}','{date_god}',{post})";
            My_Execute_Non_Query(command);
            string command2 = $"insert into Склад (Код_препарата, Количество_препаратов, Цена, Дата_доставки, Дата_изготовления, Срок_годности) values ({id_prep}, {count}, {stoim}, '{date_today}', '{date_izg}', '{date_god}')";
            My_Execute_Non_Query(command2);
        }

        private void Get_Preparate()
        {
            string command = $"select Препараты.Код_препарата, Препараты.Наименование, Категории.Категория, Препараты.Цена, Препараты.Единица_измерения, Склад.Количество_препаратов, Препараты.Единица_измерения, Препараты.Дата_производства, Препараты.Срок_годности, Поставщики.Код_поставщика from Препараты, Склад, Поставщики, Категории where Препараты.Код_препарата = Склад.Код_препарата and Препараты.Код_категории = Категории.Код_категории and Препараты.Код_поставщика = Поставщики.Код_поставщика";
            Form1 f = new Form1();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command, f.connection);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds, "Заказ");
            dataGridView2.DataSource = ds.Tables["Заказ"].DefaultView;
        }

        private void Add_Client(int id, string fam, string name, string otch, string adress, string phone)
        {
            string command = $"insert into Клиенты (Код_клиента, Фамилия, Имя, Отчество, Адрес, Телефон) values ({id},'{fam}','{name}','{otch}','{adress}','{phone}')";
            My_Execute_Non_Query(command);
        }

        private void Get_Client()
        {
            string command = $"select * from Клиенты";
            Form1 f = new Form1();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command, f.connection);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds, "Заказ");
            dataGridView2.DataSource = ds.Tables["Заказ"].DefaultView;
        }

        private void Add_Worker(int id, string fam, string name, string otch, string adress, string phone, string bithday)
        {
            string command = $"insert into Сотрудники (Код_сотрудника, Фамилия, Имя, Отчество, Адрес, Телефон, Дата_рождения) values ('{id}','{fam}','{name}','{otch}','{adress}','{phone}','{bithday}')";
            My_Execute_Non_Query(command);
        }

        private void Get_Worker()
        {
            string command = $"select * from Сотрудники";
            Form1 f = new Form1();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command, f.connection);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds, "Заказ");
            dataGridView2.DataSource = ds.Tables["Заказ"].DefaultView;
        }

        private void Add_Category(string name, string charactery, string prot )
        {
            string command = $"insert into Категории (Категория, Характеристика, Противопоказания) values ('{name}', '{charactery}', '{prot}')";
            My_Execute_Non_Query(command);
        }

        private void Get_Category()
        {
            string command = $"select * from Категории where Категории.Категория != ' ' ";
            Form1 f = new Form1();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command, f.connection);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds, "Заказ");
            dataGridView2.DataSource = ds.Tables["Заказ"].DefaultView;
        }

        private void Add_Post(string org, string fam, string name, string otch, string adress, string phone)
        {
            string command = $"insert into Поставщики(Название_организации, Фамилия, Имя, Отчество, Адрес, Контактный_телефон) values ('{org}','{fam}','{name}','{otch}','{adress}','{phone}')";
            My_Execute_Non_Query(command);
        }

        private void Get_Post()
        {
            string command = $"select * from Поставщики";
            Form1 f = new Form1();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command, f.connection);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds, "Заказ");
            dataGridView2.DataSource = ds.Tables["Заказ"].DefaultView;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Zakaz();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Zakaz();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form4 f = new Form4();
            if (f.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    int index, index_old;
                    string ID;
                    string CommandText = "DELETE FROM ";
                    index = dataGridView2.CurrentRow.Index; // № по порядку в таблице представления
                    index_old = index;
                    ID = Convert.ToString(dataGridView2[0, index].Value); // ID подаем в запрос как строку

                    switch (act_table)
                    {
                        case 1:
                            string CommandText2 = "delete from Склад, Препараты where Код_препарата " + ID;
                            My_Execute_Non_Query(CommandText2);

                            //CommandText = "delete from Препараты where Код_препарата " + ID;
                            //My_Execute_Non_Query(CommandText);

                            Get_Preparate();
                            break;
                        case 2:
                            CommandText = "delete from Клиенты where Код_клиента = " + ID;
                            My_Execute_Non_Query(CommandText);
                            Get_Client();
                            break;
                        case 3:
                            CommandText = "delete from Сотрудники where Код_сотрудника  = " + ID;
                            My_Execute_Non_Query(CommandText);
                            Get_Worker();
                            break;
                        case 4:
                            CommandText = "delete from Категории where Код_категории = " + ID;
                            My_Execute_Non_Query(CommandText);
                            Get_Category();
                            break;
                        case 5:
                            CommandText = "delete from Поставщики where Код_поставщика = " + ID;
                            My_Execute_Non_Query(CommandText);
                            Get_Post();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Данная запись используется в другой таблице!");
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form5 f = new Form5();

            int id_p;
            int id_c;
            string prep_name;

            int row;
            int row2;

            int count;
            int stoim;
            int new_prep_count;

            if (f.ShowDialog() == DialogResult.OK)
            {
                row = f.dataGridView1.CurrentCell.RowIndex; // взяли строку с dataGridView1
                int prep_count = Convert.ToInt32(f.dataGridView1[4, row].Value);
                count = Convert.ToInt32(f.numericUpDown1.Value);
                stoim = Convert.ToInt32(f.dataGridView1[3, row].Value) * count;

                if (count <= prep_count)
                {
                    prep_name = Convert.ToString(f.dataGridView1[1, row].Value);
                    new_prep_count = prep_count - count;
                    id_p = Convert.ToInt32(f.dataGridView1[0, row].Value);

                    row2 = f.dataGridView2.CurrentCell.RowIndex; // взяли строку с dataGridView2
                    id_c = Convert.ToInt32(f.dataGridView2[0, row2].Value);

                    Add_Zakaz(id_c, id_p, Convert.ToString(f.dateTimePicker1.Value), Convert.ToString(f.dateTimePicker2.Value), D.user_id, stoim,new_prep_count, prep_name, count);
                }
                else
                {
                    MessageBox.Show("Недостаточно товара!");
                }
            }
            Zakaz();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            switch (act_table)
            {
                case 1:
                    Form6 f = new Form6();
                    int row;
                    int row2;
                    int row3;
                    int id_post;
                    int id_cat;
                    int id_prep;
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        id_prep = Convert.ToInt32(f.maskedTextBox2.Text);
                        row = f.dataGridView1.CurrentCell.RowIndex; // взяли строку с dataGridView1
                        id_post = Convert.ToInt32(f.dataGridView1[0, row].Value);

                        row2 = f.dataGridView2.CurrentCell.RowIndex; // взяли строку с dataGridView1
                        id_cat = Convert.ToInt32(f.dataGridView2[0, row2].Value);

                        Add_Preparate(id_prep, f.textBox1.Text, Convert.ToInt32(f.numericUpDown1.Value),id_cat, Convert.ToInt32(f.maskedTextBox1.Text), Convert.ToInt32(f.numericUpDown2.Value), Convert.ToString(f.dateTimePicker1.Value), Convert.ToString(f.dateTimePicker2.Value), Convert.ToString(f.dateTimePicker3.Value), id_post);
                        Get_Preparate();
                    }
                    break;
                case 2:
                    Form9 fa = new Form9();
                    if (fa.ShowDialog() == DialogResult.OK)
                    {
                        row3 = fa.dataGridView1.CurrentCell.RowIndex; // взяли строку с dataGridView1
                        int id_work = Convert.ToInt32(fa.dataGridView1[0, row3].Value);
                        Add_Client(id_work, fa.textBox1.Text, fa.textBox2.Text, fa.textBox3.Text, fa.textBox4.Text, fa.textBox5.Text);
                        Get_Client();
                    }
                    break;
                case 3:
                    Form8 fi = new Form8();
                    if (fi.ShowDialog() == DialogResult.OK)
                    {
                        row3 = fi.dataGridView1.CurrentCell.RowIndex; // взяли строку с dataGridView1
                        int id_work = Convert.ToInt32(fi.dataGridView1[0, row3].Value);
                        Add_Worker(id_work, fi.textBox2.Text, fi.textBox3.Text, fi.textBox4.Text, fi.textBox5.Text, fi.textBox6.Text, Convert.ToString(fi.dateTimePicker1.Value));
                        Get_Worker();
                    }
                    break;
                case 4:
                    Form7 fo = new Form7();
                    if (fo.ShowDialog() == DialogResult.OK)
                    {
                        Add_Category(fo.textBox1.Text, fo.textBox2.Text, fo.textBox3.Text);
                        Get_Category();
                    }
                    break;
                case 5:
                    Form10 fu = new Form10();
                    if (fu.ShowDialog() == DialogResult.OK)
                    {
                        Add_Post(fu.textBox1.Text, fu.textBox2.Text, fu.textBox3.Text, fu.textBox4.Text, fu.textBox5.Text, fu.textBox6.Text);
                        Get_Post();
                    }
                    break;
            }
        }
    }
}
