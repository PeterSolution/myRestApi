using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApi.Context;
using ServerApi.Functions;
using ServerApi.Models;

namespace ServerApi.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly ContextFile _context;
        ControllFunctions controllFunctions;
        public NotificationsController(ContextFile context)
        {
            _context = context;
            controllFunctions = new ControllFunctions(_context);
        }

        // GET: api/Notifications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationDbModel>>> GetNotification()
        {
            return await _context.Notification.ToListAsync();
        }

        // GET: api/Notifications/5
        [HttpGet("id")]
        public async Task<ActionResult<NotificationDbModel>> GetNotificationDbModel(int id)
        {
            var listofnotification= await _context.Notification.ToListAsync();

            var user = await _context.User.FindAsync(id);

            List<NotificationDbModel> usernotifications = new List<NotificationDbModel>();

            if (user == null)
            {
                return NotFound();
            }
            foreach (var notification in listofnotification)
            {
                if (notification.idduser == id)
                {
                    usernotifications.Append(notification);
                }
            }
            return Ok(usernotifications);

            /*var notificationDbModel = await _context.Notification.FindAsync(id);

            if (notificationDbModel == null)
            {
                return NotFound();
            }

            return notificationDbModel;*/
        }

        // PUT: api/Notifications/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutNotificationDbModel(NotificationDbModel notificationDbModel)
        {
            using (var transaction = controllFunctions.StartTransaction())
            {
                if (NotificationDbModelExists(notificationDbModel.id))
                {
                    return BadRequest();
                }

                _context.Entry(notificationDbModel).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationDbModelExists(notificationDbModel.id))
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

        // POST: api/Notifications
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPost]
        public async Task<ActionResult<NotificationForUserDbModel>> PostNotificationDbModel(NotificationForUserDbModel notificationForUserDbModel)
        {
            _context.Notification.Add(notificationDbModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNotificationDbModel", new { id = notificationDbModel.id }, notificationDbModel);
        }*/


        private bool NotificationDbModelExists(int id)
        {
            using (var transaction = controllFunctions.StartTransaction())
            {
                return _context.Notification.Any(e => e.id == id);
            }
        }
    }
}
