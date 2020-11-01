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
        public ActionResult<List<Rent>> GetAll()
        {
            return _context.Rents.ToList();
        }


        [HttpGet("{id}", Name = "GetRent")]
        public ActionResult<Rent> GetById(int id)
        {
            var item = _context.Rents.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
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
        
        private bool checkIfCarAvailable(int CarID, DateTime DateFrom, DateTime DateTo)
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
                Rent rent = new Rent();
                rent.Car = _context.Cars.Find(rentInfo.CarID);
                rent.DateFrom = rentInfo.DateFrom;
                rent.DateTo = rentInfo.DateTo;
                //rent.User
                //var userid = _userManager.GetUserId(User);
                //var u = User;
                //bool IsAdmin = _userManager.IsInRole(User.Identity.GetUSerId(), "admin");
                _context.Rents.Add(rent);
                await _context.SaveChangesAsync();
                return Ok(rent);
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
