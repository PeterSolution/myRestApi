﻿using System.ComponentModel.DataAnnotations;

namespace ServerApi.Models
{
    public class DataDbModel
    {
        [Key]
        public int iddata { get; set; }
        public string createuser { get; set; }
        public string title { get; set; }
        public string date { get; set; }

    }
}
