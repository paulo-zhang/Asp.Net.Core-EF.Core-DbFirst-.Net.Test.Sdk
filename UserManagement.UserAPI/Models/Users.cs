using System;
using System.Collections.Generic;

namespace UserManagement.UserAPI.Models
{
    public partial class Users
    {
        public Users()
        {
            Clients = new HashSet<Clients>();
            Managers = new HashSet<Managers>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Alias { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Clients> Clients { get; set; }
        public virtual ICollection<Managers> Managers { get; set; }
    }
}
