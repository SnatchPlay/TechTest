
using System.Windows.Forms;
using WinFormsApp1.Models;
using WinFormsApp1.Repository;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        IRepository<Order> ordersRepository=new OrdersRepository();
        IRepository<User> usersRepository=new UsersRepository();
        public Form1()
        {
            InitializeComponent();
            tabPage2.Text = "UsersMenu";
            tabPage1.Text = "OrdersMenu";
            InitDataGrid1();
            InitDataGrid2();
            RedrawDatagrid1();
            RedrawDatagrid2();
            monthCalendar1.MaxSelectionCount = 1;
            monthCalendar2.MaxSelectionCount = 1;

            InitUserIDList();
        }
        private void InitDataGrid1()
        {
            dataGridView1.Columns.Add("col1", "UserID");
            dataGridView1.Columns.Add("col2", "Login");
            dataGridView1.Columns.Add("col3", "Password");
            dataGridView1.Columns.Add("col4", "FirstName");
            dataGridView1.Columns.Add("col5", "LastName");
            dataGridView1.Columns.Add("col6", "DateOfBirth");
            dataGridView1.Columns.Add("col7", "Gender");
        }
        
        private void RedrawDatagrid1()
        {
            dataGridView1.Rows.Clear();
            List<User> users = usersRepository.GetAll();

            for (int i=0;i< usersRepository.GetAll().Count; i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView1);
                row.Cells[0].Value = users[i].UserID;
                row.Cells[1].Value = users[i].Login;
                row.Cells[2].Value = users[i].Password;
                row.Cells[3].Value = users[i].FirstName;
                row.Cells[4].Value = users[i].LastName;
                row.Cells[5].Value = users[i].DateOfBirth;
                row.Cells[6].Value = users[i].Gender;
                dataGridView1.Rows.Add(row);
            }   

        }
        private void UserElementsHidingOff()
        {
            label1.Visible = true;label2.Visible = true;label3.Visible = true;
            label4.Visible = true; label5.Visible = true; label6.Visible = true;
            textBox1.Visible = true; textBox2.Visible = true;
            textBox3.Visible = true; textBox4.Visible = true;
            monthCalendar1.Visible = true;
            listBox1.Visible = true;
        }
        private void UserElementsHidingOn()
        {
            label1.Visible = false; label2.Visible = false; label3.Visible = false;
            label4.Visible = false; label5.Visible = false; label6.Visible = false;
            textBox1.Visible = false; textBox2.Visible = false;
            textBox3.Visible = false; textBox4.Visible = false;
            monthCalendar1.Visible = false;
            listBox1.Visible = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            button4.Visible = true;
            UserElementsHidingOff();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int id=Convert.ToInt32( dataGridView1.CurrentRow.Cells[0].Value);
            usersRepository.Delete(id);
            RedrawDatagrid1();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string login = textBox1.Text;
                string password = textBox2.Text;
                string firstName = textBox3.Text;
                string lastName = textBox4.Text;
                string dateOfBirth = monthCalendar1.SelectionRange.Start.ToString();
                string gender = listBox1.Text;
                User user = new User();
                user.Login = login;
                user.Password = password;
                user.FirstName = firstName;
                user.LastName = lastName;
                user.DateOfBirth = DateTime.Parse(dateOfBirth);
                user.Gender = gender;

                usersRepository.Create(user);
                textBox1.Clear(); textBox2.Clear(); textBox3.Clear(); textBox4.Clear();
                RedrawDatagrid1();
                UserElementsHidingOn();
                button4.Visible = false;
            }
            catch (Exception ex)
            {
                 MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label7.Visible = true;
            listBox2.Visible = true;
            UserElementsHidingOff();
            button5.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                User user = usersRepository.Get(Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value));

                if (listBox2.SelectedIndex == 0)
                {
                    user.Login = textBox1.Text;
                    usersRepository.Update(user);
                }
                else if (listBox2.SelectedIndex == 1)
                {
                    user.Password = textBox2.Text;
                    usersRepository.Update(user);
                }
                else if (listBox2.SelectedIndex == 2)
                {
                    user.FirstName = textBox3.Text;
                    usersRepository.Update(user);
                }
                else if (listBox2.SelectedIndex == 3)
                {
                    user.LastName = textBox4.Text;
                    usersRepository.Update(user);
                }
                else if (listBox2.SelectedIndex == 4)
                {
                    user.DateOfBirth = DateTime.Parse(monthCalendar1.SelectionRange.Start.ToString());
                    usersRepository.Update(user);
                }
                else if (listBox2.SelectedIndex == 5)
                {
                    user.Gender = listBox1.Text;
                    usersRepository.Update(user);
                }
                label7.Visible = false;
                listBox2.Visible = false;

                button5.Visible = false;
                UserElementsHidingOn();
                RedrawDatagrid1();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK);
            }
        }
        //////////////////////////////////////////////
        private void InitDataGrid2()
        {
            dataGridView2.Columns.Add("col1", "OrderID");
            dataGridView2.Columns.Add("col2", "UserID");
            dataGridView2.Columns.Add("col3", "Order Date");
            dataGridView2.Columns.Add("col4", "Order Cost");
            dataGridView2.Columns.Add("col5", "Items Description");
            dataGridView2.Columns.Add("col6", "Shipping Address");
        }
    private void RedrawDatagrid2()
        {
            dataGridView2.Rows.Clear();
            //dataGridView2.Refresh();
            List<Order> orders = ordersRepository.GetAll();

            
            for (int i = 0; i < ordersRepository.GetAll().Count; i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView2);
                row.Cells[0].Value = orders[i].OrderID;
                row.Cells[1].Value = orders[i].UsertID;
                row.Cells[2].Value = orders[i].OrderDate;
                row.Cells[3].Value = orders[i].OrderCost;
                row.Cells[4].Value = orders[i].ItemsDescription;
                row.Cells[5].Value = orders[i].ShippingAddress;
                dataGridView2.Rows.Add(row);
            }

        }
        private void InitUserIDList()
        {
            foreach(User user in usersRepository.GetAll())
            {
                listBox4.Items.Add( user.UserID);
            }
            
        }
        private void OrderElementsHidingOff()
        {
            label8.Visible = true; label9.Visible = true; label10.Visible = true;
            label11.Visible = true; label12.Visible = true; label13.Visible = true;
            textBox7.Visible = true; textBox6.Visible = true;
            textBox5.Visible = true; 
            monthCalendar2.Visible = true;
            listBox4.Visible = true;
        }
        private void OrderElementsHidingOn()
        {
            label8.Visible = false; label9.Visible = false; label10.Visible = false;
            label11.Visible = false; label12.Visible = false; label13.Visible = false;
            textBox7.Visible = false; textBox6.Visible = false;
            textBox5.Visible = false; 
            monthCalendar2.Visible = false;
            listBox4.Visible = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OrderElementsHidingOff();
            button8.Visible = true;

        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                Decimal cost = Convert.ToDecimal(textBox7.Text);
                string description = textBox6.Text;
                string address = textBox5.Text;
                string lastName = textBox4.Text;
                string date = monthCalendar2.SelectionRange.Start.ToString();
                int userId = Convert.ToInt32(listBox4.Text);
                Order order = new Order();
                order.UsertID = userId;
                order.OrderDate = DateTime.Parse(date);
                order.OrderCost = cost;
                order.ItemsDescription = description;
                order.ShippingAddress = address;

                ordersRepository.Create(order);
                textBox7.Clear(); textBox6.Clear(); textBox5.Clear();
                RedrawDatagrid2();
                OrderElementsHidingOn();
                button8.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            label8.Visible = true;
            listBox3.Visible = true;
            OrderElementsHidingOff();
            button9.Visible = true;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                Order order = ordersRepository.Get(Convert.ToInt32(dataGridView2.CurrentRow.Cells[0].Value));

                if (listBox3.SelectedIndex == 0)
                {
                    order.UsertID = Convert.ToInt32(listBox4.Text);
                    ordersRepository.Update(order);
                }
                else if (listBox3.SelectedIndex == 1)
                {
                    order.OrderCost = Convert.ToDecimal(textBox7.Text);
                    ordersRepository.Update(order);
                }
                else if (listBox3.SelectedIndex == 2)
                {
                    order.ItemsDescription = textBox6.Text;
                    ordersRepository.Update(order);
                }
                else if (listBox3.SelectedIndex == 3)
                {
                    order.ShippingAddress = textBox5.Text;
                    ordersRepository.Update(order);
                }
                else if (listBox3.SelectedIndex == 4)
                {
                    order.OrderDate = DateTime.Parse(monthCalendar2.SelectionRange.Start.ToString());
                    ordersRepository.Update(order);
                }

                label8.Visible = false;
                listBox3.Visible = false;

                button9.Visible = false;
                OrderElementsHidingOn();
                RedrawDatagrid2();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK);
            }
        }
    }
}