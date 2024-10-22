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
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ContextFile _context;
        ControllFunctions controllFunctions;

        public IMapper Mapper;

        public UsersController(ContextFile context,IMapper mapper)
        {
            _context = context;
            Mapper = mapper;
            controllFunctions = new ControllFunctions(_context);
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDbModel>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }
        [HttpGet("name")]
        public async Task<ActionResult<UserDbModel>> GetUserByLogin(string login, string password)
        {
            var user = await _context.User.FirstOrDefaultAsync(use => use.name == login);
            if (user == null)
            {
                return Forbid("selected login exist");
            }
            if (user == null || user.password != password)
            {
                return NotFound();
            }

            return Ok(user);


        }

        // GET: api/Users/5
        [HttpGet("id")]
        public async Task<ActionResult<UserDbModel>> GetUserDbModel(int id)
        {
            var userDbModel = await _context.User.FindAsync(id);

            if (userDbModel == null)
            {
                return NotFound();
            }

            return userDbModel;
        }
        [HttpGet("all")]
        public async Task<ActionResult<List<UserForShowDbModel>>> GetUserForUser()
        {

            var users = await _context.User.ToListAsync();
            UserForShowDbModel usersforconvert = new UserForShowDbModel();

            List<UserForShowDbModel> usersforuser = new List<UserForShowDbModel>();
            foreach(var item in users)
            {
                Mapper.Map(item, usersforconvert);
                usersforuser.Add(usersforconvert);

            }

            Mapper.Map(users,usersforuser);

            return usersforuser;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserDbModel(UserDbModel userDbModel)
        {
            using (var transaction = controllFunctions.StartTransaction())
            {
                if (UserDbModelExists(userDbModel.idduser))
                {
                    return BadRequest();
                }

                _context.Entry(userDbModel).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserDbModelExists(userDbModel.idduser))
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
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserDbModel>> PostUserDbModel(UserDbModel userDbModel)
        {
            using (var transaction = await controllFunctions.StartTransaction())
            {
                _context.User.Add(userDbModel);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return CreatedAtAction("GetUserDbModel", new { id = userDbModel.idduser }, userDbModel);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHideDbModel(int id)
        {
            var hideDbModel = await _context.User.FindAsync(id);
            if (hideDbModel == null)
            {
                return NotFound();
            }

            _context.User.Remove(hideDbModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool UserDbModelExists(int id)
        {
            return _context.User.Any(e => e.idduser == id);
        }
    }
}
