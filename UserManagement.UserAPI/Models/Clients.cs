using System;
using System.Collections.Generic;

namespace UserManagement.UserAPI.Models
{
    public partial class Clients
    {
        public int ClientId { get; set; }
        public int UserId { get; set; }
        public int? ManagerId { get; set; }
        public int Level { get; set; }

        public virtual Managers Manager { get; set; }
        public virtual Users User { get; set; }
    }
}
