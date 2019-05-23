using System;
using System.Collections.Generic;

namespace MyMantriDataAccessLayer.Models
{
    public partial class Admin
    {
        public int AdminId { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string SecurityAns { get; set; }

        public UserCredentials AdminNavigation { get; set; }
    }
}
