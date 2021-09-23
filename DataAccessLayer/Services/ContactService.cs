using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Services
{
    public class ContactService : IContactService
    {
        private string _connectionString;

        public ContactService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("default");
        }

        public bool Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Contact> GetAll()
        {
            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using(SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Contact";
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new Contact
                            {
                                Id = (int)reader["Id"],
                                LastName = reader["LastName"].ToString(),
                                FirstName = reader["FirstName"].ToString(),
                                Email = reader["Email"].ToString(),
                                Telephone = reader["Telephone"].ToString(),
                                IsFavorite = (bool)reader["IsFavorite"]
                            };
                        }
                    }
                }
            }
        }

        public Contact GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public int Insert(Contact c)
        {
            throw new NotImplementedException();
        }

        public bool Update(Contact c)
        {
            throw new NotImplementedException();
        }
    }
}
