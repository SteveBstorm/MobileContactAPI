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
            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using(SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Contact WHERE Id = @Id";
                    cmd.Parameters.AddWithValue("Id", Id);

                    int nbRow = cmd.ExecuteNonQuery();

                    return nbRow == 1;
                }
            }
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
            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using(SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Contact WHERE Id = @Id";

                    SqlParameter idParameter = new SqlParameter("Id", Id);
                    cmd.Parameters.Add(idParameter);

                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            return new Contact()
                            {
                                Id = (int)reader["Id"],
                                LastName = reader["LastName"].ToString(),
                                FirstName = reader["FirstName"].ToString(),
                                Email = reader["Email"].ToString(),
                                Telephone = reader["Telephone"].ToString(),
                                IsFavorite = (bool)reader["IsFavorite"]
                            };
                        }

                        return null;
                    }

                }
            }
        }

        public int Insert(Contact c)
        {
            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using(SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Contact ([FirstName],[LastName], [Email],[Telephone], [IsFavorite])"
                                    + " OUTPUT Inserted.Id"
                                    + " VALUES (@FirstName, @LastName, @Email, @Telephone, @IsFavorite);";

                    cmd.Parameters.AddWithValue("FirstName", c.FirstName);
                    cmd.Parameters.AddWithValue("LastName", c.LastName);
                    cmd.Parameters.AddWithValue("Email", c.Email);
                    cmd.Parameters.AddWithValue("Telephone", c.Telephone);
                    cmd.Parameters.AddWithValue("IsFavorite", c.IsFavorite);

                    int id = (int)cmd.ExecuteScalar();
                    return id;
                }
            }
        }

        public bool Update(int Id, Contact c)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "UPDATE Contact"
                                    + " SET [FirstName] = @FirstName,"
                                    + "     [LastName] = @LastName,"
                                    + "     [Email] = @Email,"
                                    + "     [Telephone] = @Telephone"
                                    + "     [IsFavorite] = @IsFavorite"
                                    + " WHERE Id = @Id";

                    cmd.Parameters.AddWithValue("Id", Id);
                    cmd.Parameters.AddWithValue("FirstName", c.FirstName);
                    cmd.Parameters.AddWithValue("LastName", c.LastName);
                    cmd.Parameters.AddWithValue("Email", c.Email);
                    cmd.Parameters.AddWithValue("Telephone", c.Telephone);
                    cmd.Parameters.AddWithValue("IsFavorite", c.IsFavorite);

                    return cmd.ExecuteNonQuery() == 1;
                }
            }
        }
    }
}
