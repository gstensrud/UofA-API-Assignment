using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API_Assignment.Models;
using API_Assignment.Data;
using static System.Net.Mime.MediaTypeNames;
using System.Net;
using System.Xml.Linq;
using System;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Linq;

namespace API_Assignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public ModelController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<VehicleModel> Get()
        {
            return _context.Models.ToArray();
        }

        [HttpGet("{id}")]
        public ActionResult<VehicleModel> Get(int id)
        {
            try
            {
                VehicleModel found = _context.Models.Where(x => x.ID == id).Single();
                return found;
            }
            catch
            {
                return NotFound("Sorry!! Please try again.  That VEHICLE MODEL was NOT found!!");
            }
        }

        [HttpPost]
        public ActionResult Post(int Manufacturer_ID, string name)
        {
            VehicleManufacturer test;
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Sorry! Something went horribly wrong.  Please check your info, and try again.");
            }
            CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name);
            try
            {
                test = _context.Manufacturers.Where(x => x.ID == Manufacturer_ID).Single();
            }
            catch
            {
                return NotFound("That Manufacturer ID can not be found!");
            }
            try
            {
                _context.Models.Add(new VehicleModel() { Name = name, ManufacturerID = Manufacturer_ID });
                _context.SaveChanges();
                return Ok("Successful!! Changes to the database have been made!");
            }
            catch
            {
                return BadRequest("Sorry! Something went horribly wrong.  Please check your info, and try again.");
            }
        }

        [HttpPatch]
        public ActionResult Patch(int id, string Field_to_Edit, string Field_Value)
        {
            string prop = Field_to_Edit;
            string value = Field_Value;
            VehicleModel found;
            try
            {
                found = _context.Models.Where(x => x.ID == id).Single();
            }
            catch
            {
                return NotFound("Sorry!! Please try again.  That VEHICLE MODEL was NOT found!!");
            }
            CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value);
            try
            {
                switch (CultureInfo.CurrentCulture.TextInfo.ToTitleCase(prop))
                {
                    case "Name":
                        found.Name = value;
                        break;
                    default:
                        return BadRequest("Sorry! Something went horribly wrong.  Please check your info, and try again.");
                }
                _context.SaveChanges();
                return Ok("Successful!! Changes to the database have been made!");
            }
            catch
            {
                return BadRequest("Sorry! Something went horribly wrong.  Please check your info, and try again.");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            VehicleModel found;
            try
            {
                found = _context.Models.Where(x => x.ID == id).Single();
            }
            catch
            {
                return NotFound("Sorry!! Please try again.  That VEHICLE MODEL was NOT found!!");
            }
            try
            {
                _context.Models.Remove(found);
                _context.SaveChanges();
                return Ok("Successful!! Changes to the database have been made!");
            }
            catch
            {
                return BadRequest("Sorry! Something went horribly wrong.  Please check your info, and try again.");
            }
        }
    }
}

