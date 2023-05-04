using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.Models;

namespace WinFormsApp1.Repository
{
    public class UsersRepository : IRepository<User>
    {
        List<User> UsersList;
        protected string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
        public UsersRepository()
        {
            UsersList = new List<User>();
            Read();
        }
        public void Create(User tempObj)
        {
            
            using (SqlConnection connectionSql = new SqlConnection(connStr))
            {
                connectionSql.Open();
                string CommandText = "INSERT INTO [Users]([Login],[Password],[FirstName],[LastName],[DateOfBirth],[Gender])" +
                    "VALUES(@login,@password,@firstname,@lastname,@dateofbirth,@gender)";
                SqlCommand comm = new SqlCommand(CommandText, connectionSql);
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@login", tempObj.Login);
                comm.Parameters.AddWithValue("@password", tempObj.Password);
                comm.Parameters.AddWithValue("@firstname", tempObj.FirstName);
                comm.Parameters.AddWithValue("@lastname", tempObj.LastName);
                comm.Parameters.AddWithValue("@dateofbirth", tempObj.DateOfBirth.ToString());
                comm.Parameters.AddWithValue("@gender", tempObj.Gender);
                comm.ExecuteNonQuery();

                connectionSql.Close();
            }
            using (SqlConnection connectionSql = new SqlConnection(connStr))
            {
                    connectionSql.Open();
                    string CommandText = "SELECT [UserID] FROM [Users] WHERE Login=@login";
                    SqlCommand comm = new SqlCommand(CommandText, connectionSql);
                    comm.Parameters.AddWithValue("@login", tempObj.Login);
                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        tempObj.UserID = (int)reader["UserID"];
                    }
                
            }
            UsersList.Add(tempObj);
        }

        public void Delete(int id)
        {
            User s = UsersList.First(x=>x.UserID==id);
            UsersList.Remove(s);
            using (SqlConnection connectionSql = new SqlConnection(connStr))
            {
                connectionSql.Open();
                string CommandText = "DELETE FROM Users WHERE UserID=@id";
                SqlCommand comm = new SqlCommand(CommandText, connectionSql);
                comm.Parameters.AddWithValue("@id", s.UserID);
                comm.ExecuteNonQuery();
                connectionSql.Close();
            }
        }

        public User Get(int id)
        {
            return UsersList.First(x => x.UserID == id);
        }

        public List<User> GetAll()
        {
            return UsersList;
        }

        public void Read()
        {
            using (SqlConnection connectionSql = new SqlConnection(connStr))
            {
                using (SqlCommand comm = connectionSql.CreateCommand())
                {
                    connectionSql.Open();
                    comm.CommandText = "SELECT [UserID],[Login],[Password],[FirstName],[LastName],[DateOfBirth],[Gender] FROM [Users]";

                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        User tmp = new User();
                        tmp.UserID= (int)reader["UserID"];
                        tmp.Login = (string)reader["Login"];
                        tmp.Password = (string)reader["Password"];
                        tmp.FirstName = (string)reader["FirstName"];
                        tmp.LastName = (string)reader["LastName"];
                        tmp.DateOfBirth = (DateTime)reader["DateOfBirth"];
                        tmp.Gender =(string)reader["Gender"];
                        UsersList.Add(tmp);
                    }
                }
            }
        }

        public void Refresh()
        {
            UsersList.Clear();
            Read();
        }

        public void Update(User obj)
        {
            using (SqlConnection connectionSql = new SqlConnection(connStr))
            {

                var cmd = new SqlCommand("spUpdateUsers", connectionSql);
                cmd.CommandType = CommandType.StoredProcedure;
                connectionSql.Open();
                cmd.Parameters.AddWithValue("@Id", obj.UserID);
                cmd.Parameters.AddWithValue("@login", obj.Login);
                cmd.Parameters.AddWithValue("@password", obj.Password);
                cmd.Parameters.AddWithValue("@firstName", obj.FirstName);
                cmd.Parameters.AddWithValue("@lastName", obj.LastName);
                cmd.Parameters.AddWithValue("@dateofbirth", obj.DateOfBirth.ToString());
                cmd.Parameters.AddWithValue("@gender", obj.Gender);
                cmd.ExecuteNonQuery();
                connectionSql.Close();
            }
        }
    }
}
