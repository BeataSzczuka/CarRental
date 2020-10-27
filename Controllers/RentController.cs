using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CarRental.Data;
using CarRental.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class RentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public RentController(ApplicationDbContext context)
        {
            _context = context;
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

        // POST: api/rent
        [HttpPost]
        public async Task<IActionResult> PostRent([FromBody] Rent rent)
        {
            if (ModelState.IsValid)
            {
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
