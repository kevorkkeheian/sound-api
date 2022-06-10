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
    public class SoundController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SoundController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Sound
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sound>>> GetSound()
        {
            if(!ApiKeyExists(Request)) return Unauthorized();

            if (_context.Sound == null)
            {
                return NotFound();
            }
            return await _context.Sound.Include(s => s.House).ToListAsync();
        }

        // GET: api/Sound/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sound>> GetSound(string id)
        {
            if(!ApiKeyExists(Request)) return Unauthorized();

            if (_context.Sound == null)
            {
                return NotFound();
            }
            var sound = await _context.Sound.FindAsync(id);

            if (sound == null)
            {
                return NotFound();
            }

            return sound;
        }

        // PUT: api/Sound/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSound(string id, Sound sound)
        {
            if(!ApiKeyExists(Request)) return Unauthorized();

            if (id != sound.Id)
            {
                return BadRequest();
            }

            _context.Entry(sound).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SoundExists(id))
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

        // POST: api/Sound
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Sound>> PostSound(Sound sound)
        {
            if(!ApiKeyExists(Request)) return Unauthorized();

            if (_context.Sound == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Sound'  is null.");
            }
            _context.Sound.Add(sound);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSound", new { id = sound.Id }, sound);
        }

        // DELETE: api/Sound/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSound(string id)
        {
            if(!ApiKeyExists(Request)) return Unauthorized();
            if (_context.Sound == null)
            {
                return NotFound();
            }
            var sound = await _context.Sound.FindAsync(id);
            if (sound == null)
            {
                return NotFound();
            }

            _context.Sound.Remove(sound);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SoundExists(string id)
        {
            return (_context.Sound?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool ApiKeyExists(HttpRequest request)
        {
            var key = request.Headers.FirstOrDefault(x => x.Key == "key").Value.FirstOrDefault();

            return (_context.ApiKey?.Any(e => e.Key == key)).GetValueOrDefault();
        }
    }
}
