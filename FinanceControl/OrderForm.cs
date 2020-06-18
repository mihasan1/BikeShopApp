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
    public partial class OrderForm : Form
    {
        public OrderForm()
        {
            InitializeComponent();
        }
        Point LastPoint;
        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            LastPoint.X = e.X;
            LastPoint.Y = e.Y;
        }
        private void Label3_Click(object sender, EventArgs e)
        {
            Application.Exit();

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

        private void OrderForm_Load(object sender, EventArgs e)
        {
            DB db1 = new DB();
            db1.openConnection();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT orders.ID, factory.name, product.name, colors.name, " +
                "prices.price, orders.prodCount, orders.Suma, orders.payment, users.name, users.lastname " +
                "FROM orders, factory, product, colors, prices, users " +
                "WHERE orders.ID = @oID " +
                "AND product.FK_factoryID = factory.ID " +
                "AND orders.FK_prodID = product.ID " +
                "AND product.FK_colorID = colors.ID " +
                "AND orders.FK_prodID = prices.FK_prodID " +
                "AND orders.FK_userID = users.ID", db1.getConnection());
            command.Parameters.Add("@oID", MySqlDbType.Int16).Value = veloshop.orderID;
            adapter.SelectCommand = command;
            MySqlDataReader reader = command.ExecuteReader();
            while(reader.Read())
            {
                label11.Text += reader[0].ToString();
                label2.Text += reader[1].ToString();
                label4.Text += reader[2].ToString();
                label6.Text += reader[3].ToString();
                label7.Text += reader[4].ToString();
                label8.Text += reader[5].ToString();
                label9.Text += reader[6].ToString();
                label5.Text += reader[7].ToString();
                label12.Text += "\n" + reader[8].ToString() + "\n" + reader[9].ToString();
            }
            db1.closeConnection();
        }

        private void Label3_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
