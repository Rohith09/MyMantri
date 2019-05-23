using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyMantriDataAccessLayer.Models
{
    public partial class Mantri
    {
        public Mantri()
        {
            Workdone = new HashSet<Workdone>();
        }

        public int MantriId { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public long? Mobile { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Constituency { get; set; }
        public string SecurityAns { get; set; }
        public string MantriUid { get; set; }

        public Constituency ConstituencyNavigation { get; set; }
        public UserCredentials MantriNavigation { get; set; }
        public Constituency MantriU { get; set; }
        public ICollection<Workdone> Workdone { get; set; }
    }
}
