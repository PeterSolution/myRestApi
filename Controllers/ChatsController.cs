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
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly ContextFile _context;
        ControllFunctions controllFunctions;
        IMapper mapper;

        public ChatsController(ContextFile context,IMapper mapa)
        {
            _context = context;
            controllFunctions = new ControllFunctions(_context);
            mapper = mapa;
        }

        // GET: api/Chats
        /*[HttpGet]
        public async Task<ActionResult<IEnumerable<ChatDbModel>>> GetChatDbModel()
        {
            return await _context.ChatDbModel.ToListAsync();
        }*/

        // GET: api/Chats/5
        [HttpGet("{id}")]
        public async Task<List<ChatDbModel>> GetChatDbModel(int id)
        {

            var datas = await _context.ChatDbModel.ToListAsync();
            List<ChatDbModel> wiadomosci = new List<ChatDbModel>();
            foreach(var data in datas)
            {
                if (data.chatid == id)
                {
                    wiadomosci.Add(data);
                }
            }

            return wiadomosci;

            /*var chatDbModel = await _context.ChatDbModel.FindAsync(id);

            if (chatDbModel == null)
            {
                return NotFound();
            }

            return chatDbModel;*/
        }

        // PUT: api/Chats/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutChatDbModel(int id, ChatDbModel chatDbModel)
        {
            if (id != chatDbModel.id)
            {
                return BadRequest();
            }

            _context.Entry(chatDbModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatDbModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/

        // POST: api/Chats
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ChatDbModel>> PostChatDbModel(ChatUserDbModel modelconvert)
        {
            using (var transaction = await controllFunctions.StartTransaction())
            {
                ChatDbModel chatDbModel = new ChatDbModel();
                mapper.Map( modelconvert, chatDbModel);
                _context.ChatDbModel.Add(chatDbModel);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return CreatedAtAction("GetChatDbModel", new { id = chatDbModel.id }, chatDbModel);
            }

        }
    

        // DELETE: api/Chats/5
        /*[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChatDbModel(int id)
        {
            var chatDbModel = await _context.ChatDbModel.FindAsync(id);
            if (chatDbModel == null)
            {
                return NotFound();
            }

            _context.ChatDbModel.Remove(chatDbModel);
            await _context.SaveChangesAsync();
        F
            return NoContent();
        }*/

        private bool ChatDbModelExists(int id)
        {
            return _context.ChatDbModel.Any(e => e.id == id);
        }
    }
}
