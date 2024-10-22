using System.ComponentModel.DataAnnotations;

namespace ServerApi.Models
{
    public class ChatForWho
    {
        [Key]
        public int Id { get; set; }
        public int idchat { get; set; }
        public int forwho { get; set; }
    }
}
