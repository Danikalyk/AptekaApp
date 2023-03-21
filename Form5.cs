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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            Get_Preparate();
            Get_Client();
        }

        private void Get_Preparate()
        {
            string command = $"select Препараты.Код_препарата, Препараты.Наименование, Категории.Категория, Препараты.Цена, Препараты.Единица_измерения, Склад.Количество_препаратов, Препараты.Единица_измерения, Препараты.Дата_производства, Препараты.Срок_годности, Поставщики.Код_поставщика from Препараты, Склад, Поставщики, Категории where Препараты.Код_препарата = Склад.Код_препарата and Препараты.Код_категории = Категории.Код_категории and Препараты.Код_поставщика = Поставщики.Код_поставщика";
            Form1 f = new Form1();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command, f.connection);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds, "Заказ");
            dataGridView1.DataSource = ds.Tables["Заказ"].DefaultView;
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
    }
}
