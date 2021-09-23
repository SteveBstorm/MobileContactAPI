using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoAPI.Models
{
    public static class ContactBook
    {
        public static List<Contact> contactList = new List<Contact>() {
                new Contact {
                    Id = 1,
                    Prenom = "Arthur",
                    Nom = "Pendragon",
                    Email = "arthur@kaamelott.com",
                    Telephone = "0123/45.56.67",
                    IsFavorite = true
                },
                new Contact {
                    Id = 2,
                    Prenom = "Termin",
                    Nom = "Ator",
                    Email = "terminator@skynet.com",
                    Telephone = "0987/98.87.76",
                    IsFavorite = false
                }
            };
    }
}
