using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyMantriDataAccessLayer.Models
{
    public partial class UserCredentials
    {
        public int UserId { get; set; }
        [StringLength(10), MinLength(6)]
        public string Password { get; set; }
        public int RollId { get; set; }

        public Role Roll { get; set; }
        public Admin Admin { get; set; }
        public Mantri Mantri { get; set; }
        public Voters Voters { get; set; }
    }
}
