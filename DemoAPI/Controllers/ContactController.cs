using DataAccessLayer.Interfaces;
using DataAccessLayer.Services;
using DemoAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoAPI.Tools;

namespace DemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private IContactService _service;
        public ContactController(IConfiguration config) 
        {
            _service = new ContactService(config);
        }
        //Version avec la classe static ContactBook
        //https://localhost:port/api/contact

        //[HttpGet]
        //public IActionResult GetList()
        //{
        //    return Ok(ContactBook.contactList);
        //}

        //https://localhost:port/api/contact/1

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll().Select(c => c.ToAPI()));
        }

        [HttpGet("{Id}")]
        public IActionResult GetById(int Id)
        {
            try
            {
                return Ok(ContactBook.contactList.Where(c => c.Id == Id).First());
            }
            
                
            catch(Exception e)
            {
                return NotFound("Le contact demandé n'existe pas");
            }
        }
        //https://localhost:port/api/contact
        [HttpPost]
        public IActionResult Insertion(ContactForm c)
        {
            if (ModelState.IsValid)
            {
                int maxId = ContactBook.contactList.Max(c => c.Id);
                ContactBook.contactList.Add(
                    new Contact
                    {
                        Id = maxId + 1,
                        Nom = c.LastName,
                        Prenom = c.FirstName,
                        Email = c.Email,
                        IsFavorite = c.IsFavorite,
                        Telephone = c.Telephone
                    }
                 );
                return Ok("Enregistrement effectué");
            }
            else return BadRequest();
        }
        //https://localhost:port/api/contact/1
        [HttpDelete]
        public IActionResult Suppression(int Id)
        {
            try
            {
                Contact toRemove = ContactBook.contactList.Where(c => c.Id == Id).First();
                ContactBook.contactList.Remove(toRemove);
                return Ok();
            }catch(Exception e)
            {
                return BadRequest();
            }
        }
        //https://localhost:port/api/contact/1
        [HttpPut("{Id}")]
        public IActionResult Update(int Id, [FromBody]ContactForm c)
        {
            if (ModelState.IsValid)
            {
                Contact toUpdate = ContactBook.contactList.Where(c => c.Id == Id).First();
                
                toUpdate.Email = c.Email;
                toUpdate.Prenom = c.FirstName;
                toUpdate.Nom = c.LastName;
                toUpdate.Telephone = c.Telephone;
                toUpdate.IsFavorite = c.IsFavorite;

                int index = ContactBook.contactList.IndexOf(toUpdate);

                ContactBook.contactList[index] = toUpdate;
                
                return Ok("Modification effectuée");
            }
            else return BadRequest();
        }


        //CRUD = Create Read Update Delete
    }
}
