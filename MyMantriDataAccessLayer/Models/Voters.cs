using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyMantriDataAccessLayer.Models
{
    public partial class Voters
    {
        public Voters()
        {
            Complaint = new HashSet<Complaint>();
        }

        public int VoterId { get; set; }
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

        public Constituency ConstituencyNavigation { get; set; }
        public UserCredentials Voter { get; set; }
        public ICollection<Complaint> Complaint { get; set; }
    }
}
