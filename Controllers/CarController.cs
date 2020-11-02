using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CarRental.Data;
using CarRental.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PagedList;

namespace CarRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public CarController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public ActionResult<IPagedList<Car>> GetAll(int? page, string? from, string? to)
        {
            IQueryable<Car> cars ;
            if (from != null && to != null)
            {
                var dateFrom = Convert.ToDateTime(from);
                var dateTo = Convert.ToDateTime(to);
                var rented = _context.Cars
                    .Join(_context.Rents, car => car.CarID, rent => rent.Car.CarID, (car, rent) => new { Car = car, Rent = rent })
                    .Where((o) => (o.Rent.DateFrom >= dateFrom && o.Rent.DateFrom <= dateTo) || (o.Rent.DateTo >= dateFrom && o.Rent.DateTo <= dateTo))
                    .Select((o) => o.Car.CarID)
                    .ToList();

                cars = _context.Cars.Where(c => !rented.Contains(c.CarID));
            }
            else { cars = _context.Cars; }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return Ok(cars.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet("{id}", Name = "GetCar")]  
        public ActionResult<Car> GetById(int id)
        {
            var item = _context.Cars.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        // POST: api/car
        [HttpPost]
        public async Task<IActionResult> PostCar([FromForm] Car car)
        {
            if (ModelState.IsValid)
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    car.Image = dbPath;
                    _context.Cars.Add(car);
                    await _context.SaveChangesAsync();
                    return Ok(car);
                }



            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}", Name = "DeleteCar")]
        public ActionResult<Car> DeleteById(int id)
        {
            var car = _context.Cars.Find(id);
            if (car != null) _context.Cars.Remove(car);
            _context.SaveChanges();
            return Ok();
        }
    }
}