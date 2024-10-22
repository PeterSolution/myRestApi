using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerApi.Models
{
    public class NotificationForUserDbModel
    {
        [Key]
        public int id { get; set; }
        [ForeignKey("iddata")]
        public int iddata { get; set; }
        [ForeignKey("idduser")]
        public int idduser { get; set; }
        public string date { get; set; }
        public bool isseen { get; set; }
    }
}
