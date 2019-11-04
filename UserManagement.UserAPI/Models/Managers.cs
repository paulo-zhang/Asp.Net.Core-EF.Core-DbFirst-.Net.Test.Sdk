using System;
using System.Collections.Generic;

namespace UserManagement.UserAPI.Models
{
    public partial class Managers
    {
        public Managers()
        {
            Clients = new HashSet<Clients>();
        }

        public int ManagerId { get; set; }
        public int UserId { get; set; }
        public string Position { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<Clients> Clients { get; set; }
    }
}
