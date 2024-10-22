using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApi.Context;
using ServerApi.Functions;
using ServerApi.Models;

namespace ServerApi.Controllers
{
    [Route("api/datas")]
    [ApiController]
    public class DatasController : ControllerBase
    {
        private readonly ContextFile _context;
        readonly IMapper mapper;
        ControllFunctions controllFunctions;
        public DatasController(ContextFile context,IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
            controllFunctions = new ControllFunctions(_context);
        }

        // GET: api/Data
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DataDbModel>>> GetData()
        {
            return await _context.Data.ToListAsync();
        }

        // GET: api/Data/5
        [HttpGet("id")]
        public async Task<ActionResult<DataDbModel>> GetDataDbModel(int id)
        {
            var dataDbModel = await _context.Data.FindAsync(id);

            if (dataDbModel == null)
            {
                return NotFound();
            }

            return dataDbModel;
        }

        

        // PUT: api/Data/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutDataDbModel(DataDbModel dataDbModel)
        {
            using (var transaction = await controllFunctions.StartTransaction())
            {
                if (!DataDbModelExists(dataDbModel.iddata))
                {
                    return BadRequest();
                }

                _context.Entry(dataDbModel).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataDbModelExists(dataDbModel.iddata))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                await transaction.CommitAsync();
                return NoContent();
            }
        }

        // POST: api/Data
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DataUserDbModel>> PostDataDbModel(DataUserDbModel dataUserDbModel)
        {
            using (var transaction = await controllFunctions.StartTransaction())
            {
                DataDbModel dataDbModel = new DataDbModel();
                mapper.Map(dataUserDbModel, dataDbModel);
                _context.Data.Add(dataDbModel);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return CreatedAtAction("GetDataDbModel", dataDbModel);
            }
        }


        private bool DataDbModelExists(int id)
        {
            using (var transaction = controllFunctions.StartTransaction())
            {
                return _context.Data.Any(e => e.iddata == id);
            }
        }
    }
}
