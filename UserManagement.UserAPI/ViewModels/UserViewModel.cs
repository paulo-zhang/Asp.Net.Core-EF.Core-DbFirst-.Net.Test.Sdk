using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.UserAPI.ViewModels
{
    public abstract class UserViewModel
    {
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(100)]
        public string Alias { get; set; }
        [StringLength(100)]
        public string FirstName { get; set; }
        [StringLength(100)]
        public string LastName { get; set; }
        public ManagerViewModel Manager { get; set; }
    }
}
