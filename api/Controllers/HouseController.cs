using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Models;
using Api.Attributes;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class HouseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HouseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/House
        [HttpGet]
        public async Task<ActionResult<IEnumerable<House>>> GetHouse()
        {
            if(!ApiKeyExists(Request)) return Unauthorized();

            if (_context.House == null)
            {
                return NotFound();
            }
            return await _context.House.ToListAsync();
        }

        // GET: api/House/5
        [HttpGet("{id}")]
        public async Task<ActionResult<House>> GetHouse(string id)
        {
            if(!ApiKeyExists(Request)) return Unauthorized();

            if (_context.House == null)
            {
                return NotFound();
            }
            var house = await _context.House.FindAsync(id);

            if (house == null)
            {
                return NotFound();
            }

            return house;
        }

        // PUT: api/House/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHouse(string id, House house)
        {
            if(!ApiKeyExists(Request)) return Unauthorized();

            if (id != house.Id)
            {
                return BadRequest();
            }

            _context.Entry(house).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HouseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/House
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<House>> PostHouse(House house)
        {
            if(!ApiKeyExists(Request)) return Unauthorized();

            if (_context.House == null)
            {
                return Problem("Entity set 'ApplicationDbContext.House'  is null.");
            }
            _context.House.Add(house);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHouse", new { id = house.Id }, house);
        }

        // DELETE: api/House/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHouse(string id)
        {
            if(!ApiKeyExists(Request)) return Unauthorized();
            
            if (_context.House == null)
            {
                return NotFound();
            }
            var house = await _context.House.FindAsync(id);
            if (house == null)
            {
                return NotFound();
            }

            _context.House.Remove(house);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HouseExists(string id)
        {
            return (_context.House?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool ApiKeyExists(HttpRequest request)
        {
            var key = request.Headers.FirstOrDefault(x => x.Key == "key").Value.FirstOrDefault();

            return (_context.ApiKey?.Any(e => e.Key == key)).GetValueOrDefault();
        }
    }
}
