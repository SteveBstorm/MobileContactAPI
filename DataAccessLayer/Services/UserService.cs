using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox.Database;
using System.Data.SqlClient;
using System.Data;

namespace DataAccessLayer.Services
{
    public class UserService : IUserService
    {
        private string _connectionString;

        private Connection _connection;

        public UserService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("default");

            _connection = new Connection(SqlClientFactory.Instance, _connectionString);
        }

        private User Convert(IDataReader reader)
        {
            return new User
            {
                Email = reader["Email"] is DBNull ? null : reader["Email"].ToString(),
                Id = (int)reader["Id"],
                IsAdmin = (bool)reader["IsAdmin"]
            };
        }

        public User Login(string Email, string Password)
        {
            Command cmd = new Command("LoginUser", true);
            cmd.AddParameter("Email", Email);
            cmd.AddParameter("Password", Password);

            _connection.Open();
            User ConnectedUser = _connection.ExecuteReader(cmd, Convert).FirstOrDefault();
            _connection.Close();

            return ConnectedUser;
        }

        public void Register(string Email, string Password, bool IsAdmin)
        {
            Command cmd = new Command("RegisterUser", true);
            cmd.AddParameter("Email", Email);
            cmd.AddParameter("Password", Password);
            cmd.AddParameter("IsAdmin", IsAdmin);

            _connection.Open();
            _connection.ExecuteNonQuery(cmd);
            _connection.Close();
        }
    }
}
