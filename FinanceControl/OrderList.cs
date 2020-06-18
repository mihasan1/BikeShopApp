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
    public partial class OrderList : Form
    {
        public OrderList()
        {
            InitializeComponent();
        }

        private void Label3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Label1_Click(object sender, EventArgs e)
        {
            Information info = new Information();
            this.Close();
            info.Show();
        }
        Point LastPoint;

        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            LastPoint.X = e.X;
            LastPoint.Y = e.Y;
        }

        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - LastPoint.X;
                this.Top += e.Y - LastPoint.Y;
            }
        }

        private void Label3_MouseEnter(object sender, EventArgs e)
        {
            label3.BackColor = Color.Red;
            label3.ForeColor = Color.White;
        }

        private void Label3_MouseLeave(object sender, EventArgs e)
        {
            label3.BackColor = Color.FromArgb(178, 215, 50);
            label3.ForeColor = Color.Black;
        }

        private void Label1_MouseEnter(object sender, EventArgs e)
        {
            label3.BackColor = Color.Red;
            label3.ForeColor = Color.White;
        }

        private void Label1_MouseLeave(object sender, EventArgs e)
        {
            label3.BackColor = Color.FromArgb(178, 215, 50);
            label3.ForeColor = Color.Black;
        }

        private void OrderList_Load(object sender, EventArgs e)
        {
            DB db1 = new DB();
            db1.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT orders.id, factory.name, product.name, " +
                "prices.price, orders.prodCount, orders.Suma, orders.FK_userID " +
                "FROM orders, factory, product, prices " +
                "WHERE factory.ID = product.FK_factoryID " +
                "AND product.ID = orders.FK_prodID " +
                "AND prices.FK_prodID = orders.FK_prodID " +
                "ORDER BY orders.ID", db1.getConnection());
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                dataGridView1.Rows.Add(
                    reader[0].ToString(),
                    reader[1].ToString(),
                    reader[2].ToString(),
                    reader[3].ToString(),
                    reader[4].ToString(),
                    reader[5].ToString(),
                    reader[6].ToString());
            }
            reader.Close();
            command = new MySqlCommand("SELECT SUM(Suma) FROM orders", db1.getConnection());
            label2.Text += command.ExecuteScalar().ToString();
            command = new MySqlCommand("SELECT AVG(Suma) FROM orders", db1.getConnection());
            label4.Text += command.ExecuteScalar().ToString();
            reader.Close();
            db1.closeConnection();
        }
    }
}
