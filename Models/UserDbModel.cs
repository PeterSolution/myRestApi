using System.ComponentModel.DataAnnotations;

namespace ServerApi.Models
{
    public class UserDbModel
    {
        [Key]
        public int idduser { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string place { get; set; }
    }
}
