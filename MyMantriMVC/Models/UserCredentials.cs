using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyMantriMVC.Models
{
    public class UserCredentials
    {
        public int UserId { get; set; }
        [Required]
        [StringLength(10), MinLength(6)]
        public string Password { get; set; }
        public int RollId { get; set; }
    }
}
