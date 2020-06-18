using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace FinanceControl
{
    public partial class MainCatalog : Form
    {
        public MainCatalog()
        {
            InitializeComponent();
        }

        private void MainCatalog_Load(object sender, EventArgs e)
        {
            DB db1 = new DB();
            db1.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT product.ID,factory.name,product.name,categories.name " +
                "FROM product, factory, categories " +
                "WHERE product.FK_factoryID = factory.ID " +
                "AND product.FK_categoryID = categories.ID " +
                "ORDER BY product.ID", db1.getConnection());
            MySqlDataReader reader = command.ExecuteReader();
            List<String[]> data = new List<String[]>();

            while (reader.Read())
            {
                data.Add(new string[4]);

                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
                data[data.Count - 1][2] = reader[2].ToString();
                data[data.Count - 1][3] = reader[3].ToString();
            }
            reader.Close();

            for (int i = 0; i < data.Count; i++)
                dataGridView1.Rows.Add(data[i][0], data[i][1], data[i][2], data[i][3]);

            command = new MySqlCommand("SELECT name FROM factory", db1.getConnection());
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                comboBox1.Items.Add(reader[0].ToString());
            }
            reader.Close();

            command = new MySqlCommand("SELECT name FROM categories", db1.getConnection());
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                comboBox2.Items.Add(reader[0].ToString());
            }
            reader.Close();
            db1.closeConnection();

            richTextBox1.Text = "\n";
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            DB db1 = new DB();
            db1.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT product.ID,factory.name,product.name,categories.name " +
                "FROM product, factory, categories " +
                "WHERE product.FK_factoryID = factory.ID " +
                "AND product.FK_categoryID = categories.ID " +
                "AND factory.name = @fID ORDER BY product.ID", db1.getConnection());
            if (comboBox2.Text != "Тип")
            {
                command = new MySqlCommand("SELECT product.ID,factory.name,product.name,categories.name " +
                    "FROM product, factory, categories " +
                    "WHERE product.FK_factoryID = factory.ID " +
                    "AND product.FK_categoryID = categories.ID " +
                    "AND categories.name = @cID " +
                    "AND factory.name = @fID " +
                    "ORDER BY product.ID", db1.getConnection());
                command.Parameters.Add("@cID", MySqlDbType.VarChar).Value = Convert.ToString(comboBox2.SelectedItem);
            }
            
            command.Parameters.Add("@fID", MySqlDbType.VarChar).Value = Convert.ToString(comboBox1.SelectedItem);

            MySqlDataReader reader = command.ExecuteReader();

            List<String[]> data = new List<String[]>();

            while (reader.Read())
            {
                data.Add(new string[4]);

                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
                data[data.Count - 1][2] = reader[2].ToString();
                data[data.Count - 1][3] = reader[3].ToString();

            }
            reader.Close();

            for (int i = 0; i < data.Count; i++)
                dataGridView1.Rows.Add(data[i][0], data[i][1], data[i][2], data[i][3]);
            db1.closeConnection();
        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            DB db1 = new DB();
            db1.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT product.ID,factory.name,product.name,categories.name " +
                "FROM product, factory, categories " +
                "WHERE product.FK_factoryID = factory.ID " +
                "AND product.FK_categoryID = categories.ID " +
                "AND categories.name = @cID " +
                "ORDER BY product.ID", db1.getConnection());
            if(comboBox1.Text!="Виробник")
            {
                command = new MySqlCommand("SELECT product.ID,factory.name,product.name,categories.name " +
                    "FROM product, factory, categories " +
                    "WHERE product.FK_factoryID = factory.ID " +
                    "AND product.FK_categoryID = categories.ID " +
                    "AND categories.name = @cID " +
                    "AND factory.name = @fID " +
                    "ORDER BY product.ID", db1.getConnection());
                command.Parameters.Add("@fID", MySqlDbType.VarChar).Value = Convert.ToString(comboBox1.SelectedItem);
            }
            command.Parameters.Add("@cID", MySqlDbType.VarChar).Value = Convert.ToString(comboBox2.SelectedItem);

            MySqlDataReader reader = command.ExecuteReader();

            List<String[]> data = new List<String[]>();

            while (reader.Read())
            {
                data.Add(new string[4]);

                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
                data[data.Count - 1][2] = reader[2].ToString();
                data[data.Count - 1][3] = reader[3].ToString();

            }
            reader.Close();

            for (int i = 0; i < data.Count; i++)
                dataGridView1.Rows.Add(data[i][0], data[i][1], data[i][2], data[i][3]);
            db1.closeConnection();
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            for(int i=0; i<dataGridView1.RowCount; i++)
            {
                for(int j=0; j < dataGridView1.ColumnCount; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.White;
                    dataGridView1.Rows[i].Cells[j].Style.ForeColor = Color.Black;
                }
            }

            for(int i=0; i<dataGridView1.ColumnCount; i++)
            {
                dataGridView1.Rows[e.RowIndex].Cells[i].Style.BackColor = Color.SteelBlue;
                dataGridView1.Rows[e.RowIndex].Cells[i].Style.ForeColor = Color.White;
            }
            veloshop.prodID = Convert.ToInt16(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            DB db1 = new DB();
            db1.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT materials.body,characteristics.body,characteristics.wheel,materials.brakes," +
                "characteristics.speeds,characteristics.maxWeight,characteristics.productWeight,materials.carriage,materials.rim, prices.price " +
                "FROM characteristics, materials, product, prices " +
                "WHERE materials.FK_prodID = @pID " +
                "AND characteristics.FK_prodID=@pID " +
                "AND prices.FK_prodID = @pID " +
                "ORDER BY characteristics.FK_prodID", db1.getConnection());
            command.Parameters.Add("@pID", MySqlDbType.Int16).Value = veloshop.prodID;
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                richTextBox1.Text = "Матеріал рами: " + reader[0].ToString() + "\n"
                    + "Розмір рами: " + reader[1].ToString() + "''\n"
                    + "Розмір коліс: " + reader[2].ToString() + "''\n"
                    + "Тип гальм: " + reader[3].ToString() + "\n"
                    + "Кількість передач: " + reader[4].ToString() + "\n"
                    + "Максимальна вага: " + reader[5].ToString() + " кг\n"
                    + "Вага велосипеда: " + reader[6].ToString() + " кг\n"
                    + "Матеріал каретки: " + reader[7].ToString() + "\n"
                    + "Матеріал руля: " + reader[8].ToString() + "\n"
                    + "Ціна: " + reader[9].ToString();
            }
            reader.Close();

            pictureBox1.ImageLocation = "http://kiat.org.ua/veloshop/"+veloshop.prodID.ToString()+".jpg";
            db1.closeConnection();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Auth auth = new Auth();
            this.Hide();
            auth.Show();
        }

        private void Label3_Click(object sender, EventArgs e)
        {
            Application.Exit();
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

        private void Button2_Click(object sender, EventArgs e)
        {
            int orderPCount = Convert.ToInt16(numericUpDown1.Value);
            DB db1 = new DB();
            db1.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT prod_count " +
                "FROM storage " +
                "WHERE ID_prod = @pID", db1.getConnection());
            command.Parameters.Add("@pID", MySqlDbType.Int16).Value = veloshop.prodID;
            if ((Convert.ToInt16(command.ExecuteScalar()) - orderPCount) < 1 )
            {
                MessageBox.Show("Такої кількості товару на складі немає!");
                return;
            }

            command = new MySqlCommand("UPDATE storage " +
                "SET prod_count=prod_count-@orderCount " +
                "WHERE ID_prod = @pID", db1.getConnection());
            command.Parameters.Add("@orderCount", MySqlDbType.Int16).Value = orderPCount;
            command.Parameters.Add("@pID", MySqlDbType.Int16).Value = veloshop.prodID;
            command.ExecuteNonQuery();


            command = new MySqlCommand("SELECT prices.price*@count " +
                "FROM prices " +
                "WHERE prices.FK_prodID = @pID", db1.getConnection());
            command.Parameters.Add("@count", MySqlDbType.Int16).Value = orderPCount;
            command.Parameters.Add("@pID", MySqlDbType.Int16).Value = veloshop.prodID;
            int sum = Convert.ToInt32(command.ExecuteScalar());
            
            command = new MySqlCommand("INSERT " +
                "INTO orders(FK_prodID, FK_userID, prodCount, Suma) " +
                "VALUES (@prodID, @userID, @prodCount, @sum)", db1.getConnection());
            command.Parameters.Add("@prodID", MySqlDbType.Int16).Value = veloshop.prodID;
            command.Parameters.Add("@userID", MySqlDbType.Int16).Value = veloshop.userID;
            command.Parameters.Add("@prodCount", MySqlDbType.Int16).Value = orderPCount;
            command.Parameters.Add("@sum", MySqlDbType.Int32).Value = Convert.ToInt32(sum);
            command.ExecuteNonQuery();

            command = new MySqlCommand("SELECT ID " +
                "FROM orders " +
                "WHERE FK_userID = @userID", db1.getConnection());
            command.Parameters.Add("@userID", MySqlDbType.Int16).Value = veloshop.userID;
            MySqlDataReader reader = command.ExecuteReader();
            while(reader.Read())
            {
                veloshop.orderID = Convert.ToInt16(reader[0]);
            }

            this.Hide();
            OrderForm order = new OrderForm();
            order.Show();
        }
    }
}
