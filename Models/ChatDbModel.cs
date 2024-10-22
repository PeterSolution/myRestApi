using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerApi.Models
{
    public class ChatDbModel
    {
        [Key]
        public int id { get;set; }
        public string sender { get;set; }
        public string message {  get;set; }
        [ForeignKey("iddata")]
        public int chatid {  get;set; }
    }
}
