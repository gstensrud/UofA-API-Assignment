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
    public class DealershhipController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public DealershhipController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Dealership> Get()
        {
            return _context.Dealerships.ToArray();
        }

        [HttpGet("{id}")]
        public ActionResult<Dealership> Get(int id)
        {
            try
            {
                Dealership found = _context.Dealerships.Where(x => x.ID == id).Single();
                return found;
            }
            catch
            {
                return BadRequest("Sorry! Something went horribly wrong.  Please check your info, and try again.");
            }
        }

        [HttpPost]
        public ActionResult Post(string Dealership_Name, int Manufacturer_ID, string Address, string Phone_Number)
        {
            string name =  Dealership_Name;
            int mfID = Manufacturer_ID;
            string address = Address;
            string phoneNumber = Phone_Number;
            VehicleManufacturer test;
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(address))
            {
                return BadRequest("Sorry! Something went horribly wrong.  Please check your info, and try again.");
            }
            try
            {
                test = _context.Manufacturers.Where(x => x.ID == mfID).Single();
            }
            catch
            {
                return NotFound("That Manufacturer ID can not be found!");
            }
            try
            {
                _context.Dealerships.Add(new Dealership() { Name = name, ManufacturerID = mfID, Address = address, PhoneNumber = phoneNumber });
                _context.SaveChanges();
                return Accepted("The New Dealership has been successfully added");
            }
            catch
            {
                return BadRequest("Sorry! Something went horribly wrong.  Please check your info, and try again.");
            }
        }

        [HttpPut]
        public ActionResult Put(int Dealer_ID, string? Dealer_Name, int Manufacturer_ID, string? Address, string? Phone_Number)
        {
            int id = Dealer_ID;
            string name = Dealer_Name;
            int mfID = Manufacturer_ID;
            string address = Address;
            string phoneNumber = Phone_Number;
            Dealership found;
            VehicleManufacturer test;
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(phoneNumber))
            {
                return BadRequest("Sorry! Something went horribly wrong.  Please check your info, and try again.");
            }
            try
            {
                test = _context.Manufacturers.Where(x => x.ID == mfID).Single();
            }
            catch
            {
                return NotFound("This is catching when checking for the ERROR 404 Manufacturer ID NOT found!!");
            }
            try
            {
                found = _context.Dealerships.Where(x => x.ID == id).Single();
            }
            catch
            {
                return NotFound("That Dealership ID can NOT be found!");
            }
            try
            {
                found.Name = name ?? found.Name;
                found.ManufacturerID = mfID;
                found.Address = address ?? found.Address;
                found.PhoneNumber = phoneNumber ?? found.PhoneNumber;
                _context.SaveChanges();
                return Ok("Changes have been made, and database overwrote");
            }
            catch
            {
                return BadRequest("Sorry! Something went horribly wrong.  Please check your info, and try again.");
            }
        }

        [HttpPatch]
            public ActionResult Patch(int DealershipID, string Field_to_Edit, string FieldValue)
            {

                if (string.IsNullOrWhiteSpace(Field_to_Edit) || string.IsNullOrWhiteSpace(FieldValue))
                {
                    return BadRequest("Sorry! Something went horribly wrong.  Please check your info, and try again.");
                }
                string prop = Field_to_Edit;
                int mfID;
                Dealership found;
                VehicleManufacturer test;
                try
                {
                    found = _context.Dealerships.Where(x => x.ID == DealershipID).Single();
                }
                catch
                {
                    return NotFound("The Manufacturer ID entered can NOT be found");
                }
                try
                {
                    switch (CultureInfo.CurrentCulture.TextInfo.ToTitleCase(prop))
                    {
                    case "Name":
                        found.Name = FieldValue;
                        break;
                    case "Manufacturer Id":
                        try
                        {
                            mfID = int.Parse(FieldValue);
                            test = _context.Manufacturers.Where(x => x.ID == mfID).Single();
                        }
                        catch
                        {
                            return BadRequest("That MANUFACTURER ID can NOT  be modified!!");
                        }
                        found.ManufacturerID = mfID;
                       break;
                    case "Manu Id":
                        try
                        {
                            mfID = int.Parse(FieldValue);
                            test = _context.Manufacturers.Where(x => x.ID == mfID).Single();
                        }
                        catch
                        {
                            return BadRequest("That MANUFACTURER ID can NOT  be modified!!");
                        }
                        found.ManufacturerID = mfID;
                        break;
                    case "Mfg Id":
                        try
                        {
                            mfID = int.Parse(FieldValue);
                            test = _context.Manufacturers.Where(x => x.ID == mfID).Single();
                        }
                        catch
                        {
                            return BadRequest("That MANUFACTURER ID can NOT  be modified!!");
                        }
                        found.ManufacturerID = mfID;
                        break;
                        case "Man Id":
                            try
                            {
                                mfID = int.Parse(FieldValue);
                                test = _context.Manufacturers.Where(x => x.ID == mfID).Single();
                            }
                            catch
                            {
                                return BadRequest("That MANUFACTURER ID can NOT  be modified!!");
                            }
                            found.ManufacturerID = mfID;
                            break;
                    case "Address":
                                found.Address = FieldValue;
                                break;
                            case "Phone Number":
                                found.PhoneNumber = FieldValue;
                                break;
                            default:
                                return BadRequest("There has been a CATOSTROPHIC ERROR processing your request.  Please check that you have entered the correct information and try again");
                    }
                    _context.SaveChanges();
                    return Ok("Your changes have been SAVED");                
                }
                    catch
                    {
                        return BadRequest("Sorry! Something went horribly wrong.  Please check your info, and try again.");
            }
            }

            [HttpDelete("{id}")]
            public ActionResult Delete(int id)
            {            
                Dealership found;                
                try
                {
                    found = _context.Dealerships.Where(x => x.ID == id).Single();
                }
                catch
                {
                    return NotFound("Sorry, that DEALERSHIP ID can NOT be found!");
                }
                try
                {
                    _context.Dealerships.Remove(found);
                    _context.SaveChanges();
                    return Ok("The chosen Dealership has been DELETED from the database.");
                }
                catch
                {
                    return BadRequest("Sorry! Something went horribly wrong.  Please check your info, and try again.");
            }
            }
    }
} 

