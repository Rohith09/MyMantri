using System;
using System.Collections.Generic;

namespace MyMantriDataAccessLayer.Models
{
    public partial class Role
    {
        public Role()
        {
            UserCredentials = new HashSet<UserCredentials>();
        }

        public int RoleId { get; set; }
        public string RoleType { get; set; }

        public ICollection<UserCredentials> UserCredentials { get; set; }
    }
}
