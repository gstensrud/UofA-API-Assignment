using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API_Assignment.Models;
using API_Assignment.Data;
using System.Globalization;

namespace API_Assignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManufacturerController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public ManufacturerController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<VehicleManufacturer> Get()
        {
            return _context.Manufacturers.ToArray();
        }
        [HttpGet("id")]
        public ActionResult<VehicleManufacturer> Get(string Manufacturer_ID)
        {
            int providedID;
            try
            {
                providedID = int.Parse(Manufacturer_ID);
            }
            catch
            {
                return BadRequest("Sorry! Something went horribly wrong.  Please check your info, and try again.");
            }
            try
            {
                VehicleManufacturer found = _context.Manufacturers.Where(x => x.ID == providedID).Single();
                return found;
            }
            catch
            {
                return NotFound("Sorry, We can NOT find that Manufacturer ID");
            }
        }

        [HttpPost]
        public ActionResult Post(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Sorry! Something went horribly wrong.  Please check your info, and try again.");
            }
            try
            {
                _context.Manufacturers.Add(new VehicleManufacturer() { Name = name });
                _context.SaveChanges();
                return Ok("New Manufacturer added to the Database!!");
            }
            catch
            {
                return BadRequest("Sorry! Something went horribly wrong.  Please check your info, and try again.");
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, string name)
        {
            VehicleManufacturer test;
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Sorry! Something went horribly wrong.  Please check your info, and try again.");
            }
            try
            {
                test = _context.Manufacturers.Where(x => x.ID == id).Single();
            }
            catch
            {
                return NotFound("This is catching when checking for the ERROR 404 Manufacturer ID NOT found!!");
            }
            try
            {
                test.Name = name;
                _context.SaveChanges();
                return Ok("Changes have been saved to the Database!");
            }
            catch
            {
                return BadRequest("Sorry! Something went horribly wrong.  Please check your info, and try again.");
            }
        }

        [HttpPatch]
        public ActionResult Patch(int id, string Name, string value)
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(value))
            {
                return BadRequest("Sorry! Something went horribly wrong.  Please check your info, and try again.");
            }
            VehicleManufacturer found;
            try
            {
                found = _context.Manufacturers.Where(x => x.ID == id).Single();
            }
            catch
            {
                return NotFound("Sorry, but that Vehicle manufacturer can NOT be found!!");
            }
            try
            {
                switch (CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Name))
                {
                    case "Name":
                        found.Name = value;
                        break;
                    default:
                        return BadRequest("Sorry! Something went horribly wrong.  Please check your info, and try again.");
                }
                _context.SaveChanges();
                return Ok("Thank You.  Changes have been Saved to the Database!");
            }
            catch
            {
                return BadRequest("Sorry! Something went horribly wrong.  Please check your info, and try again.");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            VehicleManufacturer found;

            try
            {
                found = _context.Manufacturers.Where(x => x.ID == id).Single();
            }
            catch
            {
                return NotFound("Sorry, but that MANUFACTURER ID can NOT be found!");
            }
            try
            {
                _context.Manufacturers.Remove(found);
                _context.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest("Sorry! Something went horribly wrong.  Please check your info, and try again.");
            }
        }
    }
}
