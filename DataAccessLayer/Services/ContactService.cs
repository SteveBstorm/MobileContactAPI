using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox.Database;

namespace DataAccessLayer.Services
{
    public class ContactService : IContactService
    {
        private string _connectionString;
        private Connection _Connection;

        public ContactService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("default");

            _Connection = new Connection(SqlClientFactory.Instance, _connectionString);
        }

        public bool Delete(int Id)
        {
            Command command = new Command("DELETE FROM Contact WHERE Id = @Id");
            command.AddParameter("Id", Id);

            _Connection.Open();
            int nbRow = _Connection.ExecuteNonQuery(command);
            _Connection.Close();

            return nbRow == 1;
        }

        private Contact Converter(IDataReader reader)
        {
            return new Contact
            {
                Id = (int)reader["Id"],
                LastName = reader["LastName"].ToString(),
                FirstName = reader["FirstName"].ToString(),
                Email = reader["Email"].ToString(),
                Telephone = reader["Telephone"].ToString(),
                IsFavorite = (bool)reader["IsFavorite"]
            };
        }

        public IEnumerable<Contact> GetAll()
        {
            Command command = new Command("SELECT * FROM Contact");


            _Connection.Open();
            IEnumerable<Contact> contacts = _Connection.ExecuteReader(command, Converter).ToList() ;
            _Connection.Close();

            return contacts;
        }

        public Contact GetById(int Id)
        {
            Command command = new Command("SELECT * FROM Contact WHERE Id = @Id");
            command.AddParameter("Id", Id);

            _Connection.Open();
            Contact contact = _Connection.ExecuteReader(command, Converter).SingleOrDefault();
            _Connection.Close();

            return contact;
        }

        public int Insert(Contact c)
        {
            Command command = new Command("INSERT INTO Contact ([FirstName],[LastName], [Email],[Telephone], [IsFavorite])"
                                       + " OUTPUT Inserted.Id"
                                       + " VALUES (@FirstName, @LastName, @Email, @Telephone, @IsFavorite);");

            command.AddParameter("FirstName", c.FirstName);
            command.AddParameter("LastName", c.LastName);
            command.AddParameter("Email", c.Email);
            command.AddParameter("Telephone", c.Telephone);
            command.AddParameter("IsFavorite", c.IsFavorite);

            _Connection.Open();
            int newId = _Connection.ExecuteScalar<int>(command);
            _Connection.Close();

            return newId;
        }

        public bool Update(int Id, Contact c)
        {
            Command command = new Command("UPDATE Contact"
                                        + " SET [FirstName] = @FirstName,"
                                        + "     [LastName] = @LastName,"
                                        + "     [Email] = @Email,"
                                        + "     [Telephone] = @Telephone,"
                                        + "     [IsFavorite] = @IsFavorite"
                                        + " WHERE Id = @Id");

            command.AddParameter("Id", Id);
            command.AddParameter("FirstName", c.FirstName);
            command.AddParameter("LastName", c.LastName);
            command.AddParameter("Email", c.Email);
            command.AddParameter("Telephone", c.Telephone);
            command.AddParameter("IsFavorite", c.IsFavorite);

            _Connection.Open();
            bool test = _Connection.ExecuteNonQuery(command) == 1;
            _Connection.Close();

            return test;
        }
    }
}
