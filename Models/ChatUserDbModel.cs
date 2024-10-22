using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerApi.Models
{
    public class ChatUserDbModel
    {
        
        public string sender { get;set; }
        public string message {  get;set; }
        public int chatid {  get;set; }
    }
}
