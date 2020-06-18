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
    public partial class Auth : Form
    {
        public Auth()
        {
            InitializeComponent();
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

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            label2.BackColor = Color.Red;
            label2.ForeColor = Color.White;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.BackColor = Color.FromArgb(178, 215, 50);
            label2.ForeColor = Color.Black;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String login = textBox1.Text;
            String pass = textBox2.Text;

            DB db = new DB();

            DataTable table1 = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT * FROM users WHERE login = @uL AND password= @uP",db.getConnection());
            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = login;
            command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = pass;

            adapter.SelectCommand = command;
            adapter.Fill(table1);

            if (table1.Rows.Count > 0)
            {
                int id = Convert.ToInt16(command.ExecuteScalar());
                veloshop.userID = id;
                command = new MySqlCommand("SELECT acc_type FROM users WHERE ID = @uID", db.getConnection());
                command.Parameters.Add("@uID", MySqlDbType.Int16).Value = veloshop.userID;
                string acc_type = command.ExecuteScalar().ToString();
                if (acc_type == "admin")
                {
                    this.Hide();
                    Information inform = new Information();
                    inform.Show();
                    db.closeConnection();
                }
                else if(acc_type == "client")
                {
                    this.Hide();
                    MainCatalog catalog = new MainCatalog();
                    catalog.Show();
                    db.closeConnection();
                }
            }
            else
                MessageBox.Show("Невірний логін або пароль!","Помилка!");
            db.closeConnection();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            RegisterForm reg1 = new RegisterForm();
            reg1.Show();     
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.BackColor = Color.FromArgb(84, 157, 187);
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackColor = Color.FromArgb(240, 247, 212);

        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            button2.BackColor = Color.FromArgb(84, 157, 187);
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.BackColor = Color.FromArgb(240, 247, 212);
        }
    }
}
