using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CarRental.Data;
using CarRental.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class RentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        public RentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<List<RentShow>> GetAll()
        {
            var currentUser = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var isAdmin = _userManager.IsInRoleAsync(currentUser, "Admin").Result;
            if (isAdmin)
            {
                return _context.Rents
                    .Select((r) => new RentShow()
                    {
                        RentID = r.RentID,
                        CarID = r.Car.CarID,
                        DateFrom = r.DateFrom,
                        DateTo = r.DateTo,
                        Username = r.User.UserName
                    }).ToList();
            }
            else
            {
                return _context.Rents
                    .Where((r) => r.User == currentUser)
                    .Select((r) => new RentShow()
                    {
                        RentID = r.RentID,
                        CarID = r.Car.CarID,
                        DateFrom = r.DateFrom,
                        DateTo = r.DateTo,
                        Username = r.User.UserName
                    }).ToList();
            }
        }


        [HttpGet("{id}", Name = "GetRent")]
        public ActionResult<Rent> GetById(int id)
        {
            var currentUser = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var isAdmin = _userManager.IsInRoleAsync(currentUser, "Admin").Result;
            if (isAdmin)
            {
                var item = _context.Rents.Find(id);
                if (item == null)
                {
                    return NotFound();
                }
                return item;
            }
            else return Unauthorized();
        }

        // /api/rent/0/availability?dateFrom=06/10/2020&dateTo=07/10/2020
        [HttpGet("{carID}/availability")]
        public ActionResult<Rent> GetIsCarAvailable(int carID, string dateFrom, string dateTo)
        {
            if (ModelState.IsValid)
            {
                var from = Convert.ToDateTime(dateFrom);
                var to = Convert.ToDateTime(dateTo);
                return Ok(checkIfCarAvailable(carID, from, to));
            }
            return BadRequest(ModelState);
        }
        
        private bool checkIfCarAvailable([FromBody] int CarID, DateTime DateFrom, DateTime DateTo)
        {
            if (_context.Cars.Where(car => car.CarID == CarID).Count() < 1) return false; 
            var rents = _context.Rents
                    .Where(r => (r.Car.CarID == CarID)
                    && ((r.DateFrom >= DateFrom && r.DateFrom <= DateTo) || (r.DateTo >= DateFrom && r.DateTo <= DateTo)))
                    .ToList();
            if (rents.Count > 0)
            {
                return false;
            }
            return true;
        }

        // POST: api/rent
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostRent([FromBody] RentInfo rentInfo)
        {
            if (ModelState.IsValid)
            {
                var from = Convert.ToDateTime(rentInfo.DateFrom);
                var to = Convert.ToDateTime(rentInfo.DateTo);
                var carid = Int32.Parse(rentInfo.carID);
                if (checkIfCarAvailable(carid, from, to))
                {
                    Rent rent = new Rent();
                    rent.Car = _context.Cars.Find(carid);
                    rent.DateFrom = from;
                    rent.DateTo = to;
                    rent.User = _userManager.FindByNameAsync(User.Identity.Name).Result;
                    _context.Rents.Add(rent);
                    await _context.SaveChangesAsync();
                    return Ok(rent);
                }
                else return BadRequest("The car is already rented");
            }
            return BadRequest(ModelState);

        }

        [HttpDelete("{id}", Name = "DeleteRent")]
        public ActionResult<Rent> DeleteById(int id)
        {
            var rent = _context.Rents.Find(id);
            if (rent != null) _context.Rents.Remove(rent);
            _context.SaveChanges();
            return Ok();
        }
    }
}
