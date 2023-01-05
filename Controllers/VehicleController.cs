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
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace API_Assignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public VehicleController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Vehicle> Get()
        {
            return _context.Vehicles.ToArray();
        }

        [HttpGet("{vin}")]
        public ActionResult<Vehicle> Get(string vin)
        {
            Vehicle found;
            try
            {
                found = _context.Vehicles.Where(x => x.VIN == vin).Single();
                return found;
            }
            catch
            {
                return NotFound("Sorry, can NOT find that VIN");
            }
        }

        [HttpPost]
        public ActionResult Post(string VIN, int Model_ID, int Dealer_ID, string trim)
        {
            if (string.IsNullOrWhiteSpace(VIN) || string.IsNullOrWhiteSpace(trim))
            {
                return BadRequest("Sorry, but YOU'VE done something wrong, and you need to figure that out and try again!");
            }
            VIN = VIN.ToUpper().Trim();
            try
            {
                _context.Vehicles.Add(new Vehicle() { VIN = VIN, ModelID = Model_ID, DealershipID = Dealer_ID, TrimLevel = trim });
                _context.SaveChanges();
                return Ok("Changes have been SAVED, and the DATABASE updated!!");
            }
            catch
            {
                return BadRequest("Sorry, but YOU'VE done something wrong, and you need to figure that out and try again!");
            }
        }
        
        [HttpPatch]
        public ActionResult Patch(string VIN, string Field_to_Edit, string FieldValue)
        {

            if (string.IsNullOrWhiteSpace(VIN) || string.IsNullOrWhiteSpace(Field_to_Edit) || string.IsNullOrWhiteSpace(FieldValue))
            {
                return BadRequest("Sorry! Something went horribly wrong.  Please check your info, and try again.");
            }
            string prop = Field_to_Edit;
            int providedID;
            Dealership test;
            VehicleModel check;
            Vehicle found;
            try
            {
                found = _context.Vehicles.Where(x => x.VIN == VIN).Single();
            }
            catch
            {
                return NotFound();
            }
            try
            {
                switch (CultureInfo.CurrentCulture.TextInfo.ToTitleCase(prop))
                {
                    case "Vin":
                        found.VIN = FieldValue;
                        break;
                    case "Trim":
                        found.TrimLevel = FieldValue;
                        break;
                    case "Model Id":
                        try
                        {
                            providedID = int.Parse(FieldValue);
                            check = _context.Models.Where(x => x.ID == providedID).Single();
                        }
                        catch
                        {
                            return BadRequest("We're sorry, but that did't work...try again!");
                        }
                        found.ModelID = providedID;
                        break;
                    case "Dealership Id":
                        try
                        {
                            providedID = int.Parse(FieldValue);
                            test = _context.Dealerships.Where(x => x.ID == providedID).Single();
                        }
                        catch
                        {
                            return BadRequest("We're sorry, but that did't work...try again!");
                        }

                        found.DealershipID = providedID;
                        break;
                    case "Dealer Id":
                        try
                        {
                            providedID = int.Parse(FieldValue);
                            test = _context.Dealerships.Where(x => x.ID == providedID).Single();
                        }
                        catch
                        {
                            return BadRequest("We're sorry, but that did't work...try again!");
                        }

                        found.DealershipID = providedID;
                        break;
                    default:
                        return BadRequest("Sorry, but YOU'VE done something wrong, and you need to figure that out and try again!");
                }
                _context.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest("Sorry, but YOU'VE done something wrong, and you need to figure that out and try again!");
            }
        }

        // DELETE 
        [HttpDelete("{vin}")]
        public ActionResult Delete(string vin)
        {
            Vehicle found;
            try
            {
                found = _context.Vehicles.Where(x => x.VIN == vin).Single();
            }
            catch
            {
                return NotFound();
            }
            try
            {
                _context.Vehicles.Remove(found);
                _context.SaveChanges();
                return Ok("Database updated!");
            }
            catch
            {
                return BadRequest("Sorry, but YOU'VE done something wrong, and you need to figure that out and try again!");
            }
        }
    }
}
