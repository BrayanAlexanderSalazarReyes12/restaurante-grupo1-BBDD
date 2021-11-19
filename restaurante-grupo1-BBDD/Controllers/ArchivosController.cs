using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practica.models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Practica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArchivosController : ControllerBase
    {
        [HttpPost]
        public IActionResult AddNews(IFormFile image)
        {
           
            if (image != null)
            {

                //Set Key Name
                string ImageName = image.FileName;

                //Get url To Save
                string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "C:\\Users\\braya\\OneDrive\\Escritorio\\colnodo\\modulo4\\proyecto-de-restaurante-grupo-1-react\\public\\assets\\ensaladas", ImageName);

                using (var stream = new FileStream(SavePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }
            }
            return null;
        }
    }
}
