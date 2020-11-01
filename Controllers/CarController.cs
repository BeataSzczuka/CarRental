using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CarRental.Data;
using CarRental.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public CarController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public ActionResult<List<Car>> GetAll()
        {
            return _context.Cars.ToList();
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