using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IContactService
    {
        IEnumerable<Contact> GetAll();
        Contact GetById(int Id);
        int Insert(Contact c);
        bool Delete(int Id);
        bool Update(Contact c);
    }
}
