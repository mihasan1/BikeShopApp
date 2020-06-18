using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinanceControl
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label7_MouseEnter(object sender, EventArgs e)
        {
            label7.BackColor = Color.Red;
            label7.ForeColor = Color.White;
        }

        private void label7_MouseLeave(object sender, EventArgs e)
        {
            label7.BackColor = Color.FromArgb(178, 215, 50);
            label7.ForeColor = Color.Black;
        }

        Point LastPoint;
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            LastPoint.X = e.X;
            LastPoint.Y = e.Y;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - LastPoint.X;
                this.Top += e.Y - LastPoint.Y;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "")
            {
                MessageBox.Show("Всі поля мають бути заповнені!", "Помилка!");
                return;
            }
            if (checkUser())
                return;
            DB db1 = new DB();
            MySqlCommand command = new MySqlCommand("INSERT " +
                "INTO users(login, password, name, lastname, email) " +
                "VALUES (@uLogin,@uPass,@uName,@uLastName,@eMail)",db1.getConnection());
            command.Parameters.Add("@uName", MySqlDbType.VarChar).Value = textBox1.Text;
            command.Parameters.Add("@uLastName", MySqlDbType.VarChar).Value = textBox2.Text;
            command.Parameters.Add("@uLogin", MySqlDbType.VarChar).Value = textBox3.Text;
            command.Parameters.Add("@eMail", MySqlDbType.VarChar).Value = textBox4.Text;
            command.Parameters.Add("@uPass", MySqlDbType.VarChar).Value = textBox5.Text;

            db1.openConnection();

            if (command.ExecuteNonQuery() == 1)
                MessageBox.Show("Акаунт створено!", "Успіх!");
            else
                MessageBox.Show("Акаунт не створено!", "Помилка!");

            db1.closeConnection();
        }

        public Boolean checkUser()
        {
            DB db = new DB();

            DataTable table1 = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * from users where login= @uL", db.getConnection());
            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = textBox3.Text;

            adapter.SelectCommand = command;
            adapter.Fill(table1);

            if (table1.Rows.Count > 0)
            {
                MessageBox.Show("Цей нікнейм зайнятий!","Помилка!");
                return true;
            }
            else
                return false;
        }
    }
}
