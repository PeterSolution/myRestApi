using System.ComponentModel.DataAnnotations;

namespace ServerApi.Models
{
    public class UserChatForWho
    {
        public int idchat { get; set; }
        public int forwho { get; set; }
    }
}
