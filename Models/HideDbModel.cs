using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerApi.Models
{
    public class HideDbModel
    {
        [Key]
        public int idhide { get; set; }
        [ForeignKey("idduser")]
        public int iduser {  get; set; }
        [ForeignKey("iddata")]
        public int iddata { get; set; }
        public bool hide { get; set; }
    }
}
