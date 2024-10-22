using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApi.Context;
using ServerApi.Models;

namespace ServerApi.Controllers
{
    [Route("api/hide")]
    [ApiController]
    public class HideDbModelsController : ControllerBase
    {
        private readonly ContextFile _context;

        public HideDbModelsController(ContextFile context)
        {
            _context = context;
        }

        // GET: api/HideDbModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HideDbModel>>> GetHideDbModel()
        {
            return await _context.HideDbModel.ToListAsync();
        }

        // GET: api/HideDbModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HideDbModel>> GetHideDbModel(int id)
        {
            var hideDbModel = await _context.HideDbModel.FindAsync(id);

            if (hideDbModel == null)
            {
                return NotFound();
            }

            return hideDbModel;
        }

        // PUT: api/HideDbModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHideDbModel(int id, HideDbModel hideDbModel)
        {
            if (id != hideDbModel.idhide)
            {
                return BadRequest();
            }

            _context.Entry(hideDbModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HideDbModelExists(id))
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

        // POST: api/HideDbModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HideDbModel>> PostHideDbModel(HideDbModel hideDbModel)
        {
            _context.HideDbModel.Add(hideDbModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHideDbModel", new { id = hideDbModel.idhide }, hideDbModel);
        }

        // DELETE: api/HideDbModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHideDbModel(int id)
        {
            var hideDbModel = await _context.HideDbModel.FindAsync(id);
            if (hideDbModel == null)
            {
                return NotFound();
            }

            _context.HideDbModel.Remove(hideDbModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HideDbModelExists(int id)
        {
            return _context.HideDbModel.Any(e => e.idhide == id);
        }
    }
}
