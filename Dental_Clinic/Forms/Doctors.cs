﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WinFormsApp1
{
    public partial class Doctors : Form
    {
        private Sqliteclass mydb = null;
        private string sPath = @"C:\Users\user\source\repos\WinFormsApp1\WinFormsApp1\mybd.db"; // изменяйте путь к переменной, если путь к базе данных отличается (сама база данных находится в папке WinFormsApp1 и называется mybd )
        private string sSql = string.Empty;
        private string name;
        public Doctors()
        {
            InitializeComponent();
        }
        private void Doctors_Load(object sender, EventArgs e)
        {
            Text = "Врачи";
            mydb = new Sqliteclass();
            sSql = "SELECT * FROM doctor";
            DataRow[] datarows = mydb.drExecute(sPath, sSql);
            if (datarows == null)
            {
                Text = "Ошибка чтения!";
                mydb = null;
                return;
            }
            foreach (DataRow dr in datarows)
            {
                listBox1.Items.Add(dr["name"].ToString());
            }
        }
        private void Doctors_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form ifrm = Application.OpenForms[0];
            ifrm.StartPosition = FormStartPosition.Manual;
            ifrm.Left = this.Left;
            ifrm.Top = this.Top;
            ifrm.Show();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string it = listBox1.SelectedItem.ToString().Trim();
                mydb = new Sqliteclass();
                sSql = "DELETE FROM doctor WHERE name LIKE '%" + it + "%'";
                if (mydb.iExecuteNonQuery(sPath, sSql,1) == 0)
                {
                    Text = "Ошибка удаления!";
                    mydb = null;
                    return;
                }
                mydb = null;
                Text = "Запись удалена из БД!";
                listBox1.Items.Clear();
                mydb = new Sqliteclass();
                sSql = "SELECT * FROM doctor";
                DataRow[] datarows = mydb.drExecute(sPath, sSql);
                if (datarows == null)
                {
                    Text = "Ошибка чтения!";
                    mydb = null;
                    return;
                }
                foreach (DataRow dr in datarows)
                {
                    listBox1.Items.Add(dr["name"].ToString());
                }
            }
            catch
            {
                MessageBox.Show("Вы не выбрали строку для удаления", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            name = Convert.ToString(textBox1.Text.Trim());
            if (name == "") { MessageBox.Show("Введите имя врача", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            mydb = new Sqliteclass();
            sSql = @"INSERT INTO doctor (name) VALUES ('" + name + "');";
            //Проверка работы
            if (mydb.iExecuteNonQuery(sPath, sSql,1) == 0)
            {
                Text = "Ошибка записи!";
                mydb = null;
                return;
            }
            else
            {
                Text = "Доктор добавлен в базу!";
                textBox1.Text = "";
                listBox1.Items.Add(name.ToString().Trim());
            }
            mydb = null;
        }
    }
}
