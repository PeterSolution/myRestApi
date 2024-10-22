using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApi.Context;
using ServerApi.Functions;
using ServerApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServerApi.Controllers
{
    [Route("api/ChatForWhoes")]
    [ApiController]
    public class ChatForWhoesController : ControllerBase
    {

        private readonly ContextFile _context;
        ControllFunctions controllFunctions;
        IMapper mapper;
        public ChatForWhoesController(ContextFile context, IMapper mapa) 
        {
            _context = context;
            mapper=mapa;
            controllFunctions = new ControllFunctions(_context);
        }

        // GET: api/<ChatForWhoesController>
        [HttpGet]
        public async Task<List<ChatForWho>> Get()
        {
            var datas=_context.chatForWho.ToListAsync();
            return await datas;
        }

        // GET api/<ChatForWhoesController>/5
        [HttpGet("{id}")]
        public async Task<List<ChatForWho>> Get(int id)
        {
            var datas = await _context.chatForWho.ToListAsync();
            List<ChatForWho> result = new List<ChatForWho>();
            foreach (var data in datas)
            {
                if (data.forwho == id)
                {
                    result.Add(data);
                }
            }
            return result;
        }

        // POST api/<ChatForWhoesController>
        [HttpPost]
        public async Task<ActionResult<ChatForWho>> PostChatForWho(UserChatForWho modelconvert)
        {
            using (var transaction = await controllFunctions.StartTransaction())
            {
                ChatForWho chatForWho = new ChatForWho();
                mapper.Map(modelconvert, chatForWho);
                _context.chatForWho.Add(chatForWho);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return CreatedAtAction(nameof(Get), new { id = chatForWho.Id }, chatForWho);
            }

        }

        // PUT api/<ChatForWhoesController>/5
        /*[HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }*/

        // DELETE api/<ChatForWhoesController>/5
        [HttpDelete("{id}")]
        public async Task<string> Delete(int id)
        {
            var model = await _context.chatForWho.FindAsync(id);
            if (model == null)
            {
                return "Record with id do not exist";
            }

            _context.chatForWho.Remove(model);
            await _context.SaveChangesAsync();

            return "Deleted";
        }
    }
}
