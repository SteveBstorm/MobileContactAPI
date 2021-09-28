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
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace DemoAPI.Controllers
{
   // [Authorize("user")]
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private IContactService _service;

        public ContactController(IContactService ContactService) 
        {
            _service = ContactService;
        }

        //https://localhost:port/api/contact
        [Authorize("user")]
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_service.GetAll().Select(c => c.ToAPI()));
            }
            catch(Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        //https://localhost:port/api/contact/1
        
        [HttpGet("{Id}")]
        public IActionResult GetById(int Id)
        {
            try
            {
                Contact contact = _service.GetById(Id).ToAPI();

                if(contact != null) 
                    return Ok(contact);

                return NotFound("Le contact demandé n'existe pas");
            }
            catch(Exception e)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError);
            }
        }

        //https://localhost:port/api/contact
        [Authorize("admin")]
        [HttpPost]
        public IActionResult Insertion(ContactForm c)
        {
            if (ModelState.IsValid)
            {
                int newId = _service.Insert(c.ToDAL());

                return Ok($"Enregistrement effectué, l'id est {newId}");
            }
            else return BadRequest();

        }

        //https://localhost:port/api/contact/1
        [Authorize("admin")]
        [HttpDelete("{id}")]
        public IActionResult Suppression(int Id)
        {
            if(_service.Delete(Id))
            {
                return NoContent();
            }
            return BadRequest();
        }

        //https://localhost:port/api/contact/1
        [HttpPut("{Id}")]
        public IActionResult Update(int Id, [FromBody]ContactForm c)
        {
            if (ModelState.IsValid)
            {
                if(_service.Update(Id, c.ToDAL()))
                {
                    return Ok("Modification effectuée");
                }
                return StatusCode((int)HttpStatusCode.InternalServerError);
                
            }
            else return BadRequest();
        }


        //CRUD = Create Read Update Delete
    }
}
