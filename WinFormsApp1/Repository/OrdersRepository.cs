using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.Models;

namespace WinFormsApp1.Repository
{
    public class OrdersRepository : IRepository<Order>
    {
        List<Order> OrdersList;
        protected string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
        public OrdersRepository()
        {
            OrdersList = new List<Order>();
            Read();
        }
        public void Create(Order tempObj)
        {

            using (SqlConnection connectionSql = new SqlConnection(connStr))
            {
                connectionSql.Open();
                string CommandText = "INSERT INTO [Orders]([UserID],[OrderDate],[OrderCost],[ItemsDescription],[ShippingAddress])" +
                    "VALUES(@userid,@orderdate,@ordercost,@itemsdescription,@shippingaddress)";
                SqlCommand comm = new SqlCommand(CommandText, connectionSql);
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@userid", tempObj.UsertID);
                comm.Parameters.AddWithValue("@orderdate", tempObj.OrderDate.ToString());
                comm.Parameters.AddWithValue("@ordercost", tempObj.OrderCost);
                comm.Parameters.AddWithValue("@itemsdescription", tempObj.ItemsDescription);
                comm.Parameters.AddWithValue("@shippingaddress", tempObj.ShippingAddress);
                
                comm.ExecuteNonQuery();

                connectionSql.Close();
            }
            using (SqlConnection connectionSql = new SqlConnection(connStr))
            {
                connectionSql.Open();
                string CommandText = "SELECT [OrderID] FROM [Orders] WHERE UserID=@userid AND OrderDate=@orderdate";
                SqlCommand comm = new SqlCommand(CommandText, connectionSql);
                comm.Parameters.AddWithValue("@userid", tempObj.UsertID);
                comm.Parameters.AddWithValue("@orderdate", tempObj.OrderDate.ToString());
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    tempObj.OrderID = (int)reader["OrderID"];
                }

            }
            OrdersList.Add(tempObj);
        }

        public void Delete(int id)
        {
            Order s = OrdersList.First(x => x.OrderID == id);
            OrdersList.Remove(s);
            using (SqlConnection connectionSql = new SqlConnection(connStr))
            {
                connectionSql.Open();
                string CommandText = "DELETE FROM Orders WHERE OrderID=@id";
                SqlCommand comm = new SqlCommand(CommandText, connectionSql);
                comm.Parameters.AddWithValue("@id", s.OrderID);
                comm.ExecuteNonQuery();
                connectionSql.Close();
            }
        }

        public Order Get(int id)
        {
            return OrdersList.First(x => x.OrderID == id);
        }

        public List<Order> GetAll()
        {
            return OrdersList;
        }

        public void Read()
        {
            using (SqlConnection connectionSql = new SqlConnection(connStr))
            {
                using (SqlCommand comm = connectionSql.CreateCommand())
                {
                    connectionSql.Open();
                    comm.CommandText = "SELECT [OrderID],[UserID],[OrderDate],[OrderCost],[ItemsDescription],[ShippingAddress] FROM [Orders]";

                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        Order tmp = new Order();
                        tmp.OrderID = (int)reader["OrderID"];
                        tmp.UsertID = (int)reader["UserID"];
                        tmp.OrderDate = (DateTime)reader["OrderDate"];
                        tmp.OrderCost = (Decimal)reader["OrderCost"];
                        tmp.ItemsDescription = (string)reader["ItemsDescription"];
                        tmp.ShippingAddress = (string)reader["ShippingAddress"];
                        
                        OrdersList.Add(tmp);
                    }
                }
            }
        }

        public void Refresh()
        {
            OrdersList.Clear();
            Read();
        }

        public void Update(Order obj)
        {
            using (SqlConnection connectionSql = new SqlConnection(connStr))
            {

                var cmd = new SqlCommand("spUpdateOrders", connectionSql);
                cmd.CommandType = CommandType.StoredProcedure;
                connectionSql.Open();
                cmd.Parameters.AddWithValue("@Id", obj.OrderID);
                cmd.Parameters.AddWithValue("@userId", obj.UsertID);
                cmd.Parameters.AddWithValue("@orderdate", obj.OrderDate.ToString());
                cmd.Parameters.AddWithValue("@ordercost", obj.OrderCost);
                cmd.Parameters.AddWithValue("@itemsdescription", obj.ItemsDescription);
                cmd.Parameters.AddWithValue("@shippingaddress", obj.ShippingAddress);
                cmd.ExecuteNonQuery();
                connectionSql.Close();
            }
        }
    }
}
