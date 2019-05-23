using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyMantriMVC.Models
{
    public class User
    {
        public int VoterId { get; set; }
        public int MantriId { get; set; }
        [Required(ErrorMessage = "Name is mandatory.")]
        public string Name { get; set; }
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email address.")]
        [Required(ErrorMessage = "EmailId is mandatory.")]  
        public string EmailId { get; set; }
        [Range(1000000000, 9999999999,
            ErrorMessage = "Mobile number must be of 10 digits.")]
        public long? Mobile { get; set; }
        [Required(ErrorMessage = "Date of Birth is mandatory.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Gender is mandatory.")]
        [RegularExpression("^M$|^F$|^O$", ErrorMessage = "Gender should be M or F or O")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Address is mandatory.")]
        public string Address { get; set; }
        public string Constituency { get; set; }
        [Required(ErrorMessage = "Security Answer is mandatory.")]
        public string SecurityAns { get; set; }
        public string MantriUid { get; set; }
        [StringLength(16,ErrorMessage ="Invalid Password",MinimumLength = 8)]
        [Required(ErrorMessage = "Password is mandatory.")]
        public string Password { get; set; }
        [ScaffoldColumn(false)]
        public int RollId { get; set; }
    }
}
