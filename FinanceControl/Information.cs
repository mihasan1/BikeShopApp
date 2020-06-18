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
    public partial class Information : Form
    {
        public Information()
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

        private void label2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            label2.BackColor = Color.Red;
            label2.ForeColor = Color.White;
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.BackColor = Color.FromArgb(178, 215, 50);
            label2.ForeColor = Color.Black;
        }

        private void Information_Load(object sender, EventArgs e)
        {
            DB db1 = new DB();
            db1.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT name FROM factory", db1.getConnection());
            MySqlDataReader reader = command.ExecuteReader();

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

            command = new MySqlCommand("SELECT name FROM colors", db1.getConnection());
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                comboBox3.Items.Add(reader[0].ToString());
            }
            reader.Close();

            db1.closeConnection();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Виробник" || comboBox2.Text == "Тип" || comboBox3.Text == "Колір" )
            {
                MessageBox.Show("Всі поля мають бути заповнені!", "Помилка!");
                return;
            }
            DB db1 = new DB();
            db1.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT colors.ID, factory.ID, categories.ID " +
                "FROM colors,factory,categories " +
                "WHERE colors.name=@Col " +
                "AND factory.name=@Fac " +
                "AND categories.name = @Cat ", db1.getConnection());
            command.Parameters.Add("@Col", MySqlDbType.VarChar).Value = Convert.ToString(comboBox3.SelectedItem);
            command.Parameters.Add("@Fac", MySqlDbType.VarChar).Value = Convert.ToString(comboBox1.SelectedItem);
            command.Parameters.Add("@Cat", MySqlDbType.VarChar).Value = Convert.ToString(comboBox2.SelectedItem);
            MySqlDataReader reader = command.ExecuteReader();
            int[] param = new int[3];
            
            while(reader.Read())
            {
                param[0] = Convert.ToInt16(reader[0]);
                param[1] = Convert.ToInt16(reader[1]);
                param[2] = Convert.ToInt16(reader[2]);
            }
            reader.Close();
            command = new MySqlCommand("INSERT " +
                "INTO product(name, FK_colorID, FK_factoryID, FK_categoryID) " +
                "VALUES(@name,@color,@factory,@category)", db1.getConnection());
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = Convert.ToString(textBox1.Text);
            command.Parameters.Add("@color", MySqlDbType.Int16).Value = Convert.ToInt16(param[0]);
            command.Parameters.Add("@factory", MySqlDbType.Int16).Value = Convert.ToInt16(param[1]);
            command.Parameters.Add("@category", MySqlDbType.Int16).Value = Convert.ToInt16(param[2]);
            command.ExecuteNonQuery();

            command = new MySqlCommand("SELECT ID FROM product WHERE name = @name", db1.getConnection());
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = Convert.ToString(textBox1.Text);
            int IDprod = Convert.ToInt16(command.ExecuteScalar());

            command = new MySqlCommand("INSERT " +
                "INTO characteristics(FK_prodID, wheel, body, speeds, maxWeight, productWeight) " +
                "VALUES (@pID,@wheel,@body,@speeds,@maxWeight,@pWeight)", db1.getConnection());
            command.Parameters.Add("@pID", MySqlDbType.Int16).Value = IDprod;
            command.Parameters.Add("@wheel", MySqlDbType.VarChar).Value = Convert.ToString(textBox4.Text);
            command.Parameters.Add("@body", MySqlDbType.VarChar).Value = Convert.ToString(textBox3.Text);
            command.Parameters.Add("@speeds", MySqlDbType.VarChar).Value = Convert.ToString(textBox7.Text);
            command.Parameters.Add("@maxWeight", MySqlDbType.VarChar).Value = Convert.ToString(textBox8.Text);
            command.Parameters.Add("@pWeight", MySqlDbType.VarChar).Value = Convert.ToString(textBox9.Text);
            command.ExecuteNonQuery();


            command = new MySqlCommand("INSERT " +
                "INTO materials(FK_prodID, brakes, body, carriage, rim) " +
                "VALUES(@pID, @brakes, @body, @carriage, @rim)", db1.getConnection());
            command.Parameters.Add("@pID", MySqlDbType.Int16).Value = IDprod;
            command.Parameters.Add("@brakes", MySqlDbType.VarChar).Value = Convert.ToString(textBox6.Text);
            command.Parameters.Add("@body", MySqlDbType.VarChar).Value = Convert.ToString(textBox5.Text);
            command.Parameters.Add("@carriage", MySqlDbType.VarChar).Value = Convert.ToString(textBox10.Text);
            command.Parameters.Add("@rim", MySqlDbType.VarChar).Value = Convert.ToString(textBox11.Text);
            command.ExecuteNonQuery();

            command = new MySqlCommand("INSERT " +
                "INTO prices(price, FK_prodID) " +
                "VALUES(@price, @pID)", db1.getConnection());
            command.Parameters.Add("@pID", MySqlDbType.Int16).Value = IDprod;
            command.Parameters.Add("@price", MySqlDbType.Int32).Value = Convert.ToInt32(textBox2.Text);
            command.ExecuteNonQuery();

            command = new MySqlCommand("INSERT " +
                "INTO storage(ID_prod, prod_count) " +
                "VALUES (@pID, @pCount)", db1.getConnection());
            command.Parameters.Add("@pID", MySqlDbType.Int16).Value = IDprod;
            command.Parameters.Add("@pCount", MySqlDbType.Int16).Value = Convert.ToInt16(textBox12.Text);
            command.ExecuteNonQuery();

            db1.closeConnection();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            DB db1 = new DB();
            db1.openConnection();
            MySqlCommand command = new MySqlCommand("INSERT " +
                "INTO factory(name, city, addres, email) " +
                "VALUES (@n, @c, @a, @e)", db1.getConnection());
            command.Parameters.Add("@n", MySqlDbType.VarChar).Value = Convert.ToString(textBox13.Text);
            command.Parameters.Add("@c", MySqlDbType.VarChar).Value = Convert.ToString(textBox14.Text);
            command.Parameters.Add("@a", MySqlDbType.VarChar).Value = Convert.ToString(textBox15.Text);
            command.Parameters.Add("@e", MySqlDbType.VarChar).Value = Convert.ToString(textBox16.Text);
            command.ExecuteNonQuery();
            db1.closeConnection();
        }

        private void Label3_Click(object sender, EventArgs e)
        {
            this.Close();
            MainCatalog mainCat = new MainCatalog();
            mainCat.Show();
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
       

        private void TextboxClick(object sender, MouseEventArgs e)
        {
            TextBox txb = (TextBox)sender;
            int n = System.Convert.ToInt16(txb.Tag);
            switch (n)
            {
                case 0:
                    textBox1.Text = "";
                    break;
                case 1:
                    textBox2.Text = "";

                    break;
                case 2:
                    textBox3.Text = "";

                    break;
                case 3:
                    textBox4.Text = "";

                    break;
                case 4:
                    textBox5.Text = "";

                    break;
                case 5:
                    textBox6.Text = "";

                    break;
                case 6:
                    textBox7.Text = "";

                    break;
                case 7:
                    textBox8.Text = "";

                    break;
                case 8:
                    textBox9.Text = "";

                    break;
                case 9:
                    textBox10.Text = "";

                    break;
                case 10:
                    textBox11.Text = "";

                    break;
                case 11:
                    textBox12.Text = "";

                    break;
                case 12:
                    textBox13.Text = "";

                    break;
                case 13:
                    textBox14.Text = "";

                    break;
                case 14:
                    textBox15.Text = "";

                    break;
                case 15:
                    textBox16.Text = "";

                    break;
                case 16:
                    textBox17.Text = "";

                    break;
                case 17:
                    textBox18.Text = "";

                    break;
                default:
                    break;
            }
        }

        private void TextBox1_MouseLeave(object sender, EventArgs e)
        {
            TextBox txb = (TextBox)sender;
            int n = System.Convert.ToInt16(txb.Tag);
            switch (n)
            {
                case 0:
                    if (textBox1.Text == "")
                    textBox1.Text = "Модель";
                    break;
                case 1:
                    if (textBox2.Text == "")
                    textBox2.Text = "Ціна";

                    break;
                case 2:
                    if (textBox3.Text == "")
                        textBox3.Text = "Розмір рами";
                    break;
                case 3:
                    if (textBox4.Text == "")
                        textBox4.Text = "Розмір коліс";

                    break;
                case 4:
                    if (textBox5.Text == "")
                        textBox5.Text = "Матеріал рами";

                    break;
                case 5:
                    if (textBox6.Text == "")
                        textBox6.Text = "Тип гальм";

                    break;
                case 6:
                    if (textBox7.Text == "")
                        textBox7.Text = "К-ть передач";

                    break;
                case 7:
                    if (textBox8.Text == "")
                        textBox8.Text = "Макс. вага";

                    break;
                case 8:
                    if (textBox9.Text == "")
                        textBox9.Text = "Вага велосипеда";

                    break;
                case 9:
                    if (textBox10.Text == "")
                        textBox10.Text = "Матеріал каретки";

                    break;
                case 10:
                    if (textBox11.Text == "")
                        textBox11.Text = "Матеріал руля";

                    break;
                case 11:
                    if (textBox12.Text == "")
                        textBox12.Text = "Кількість";

                    break;
                case 12:
                    if (textBox13.Text == "")
                        textBox13.Text = "Назва фірми";

                    break;
                case 13:
                    if (textBox14.Text == "")
                        textBox14.Text = "Місто";

                    break;
                case 14:
                    if (textBox15.Text == "")
                        textBox15.Text = "Адреса";

                    break;
                case 15:
                    if (textBox16.Text == "")
                        textBox16.Text = "Ел. пошта";

                    break;
                case 16:
                    if (textBox17.Text == "")
                        textBox17.Text = "Колір";
                    break;
                case 17:
                    if (textBox18.Text == "")
                        textBox18.Text = "Тип";
                    break;

                default:
                    break;
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            DB db1 = new DB();
            db1.openConnection();
            MySqlCommand command = new MySqlCommand("INSERT INTO colors(name) VALUES (@c)", db1.getConnection());
            command.Parameters.Add("@c", MySqlDbType.VarChar).Value = Convert.ToString(textBox17.Text);
            command.ExecuteNonQuery();
            db1.closeConnection();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            DB db1 = new DB();
            db1.openConnection();
            MySqlCommand command = new MySqlCommand("INSERT INTO categories(name) VALUES (@c)", db1.getConnection());
            command.Parameters.Add("@c", MySqlDbType.VarChar).Value = Convert.ToString(textBox18.Text);
            command.ExecuteNonQuery();
            db1.closeConnection();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            OrderList list = new OrderList();
            this.Hide();
            list.Show();
        }
    }
}
