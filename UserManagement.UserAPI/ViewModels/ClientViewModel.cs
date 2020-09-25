using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.UserAPI.ViewModels
{
    public class ClientViewModel : UserViewModel
    {
        public int ClientId { get; set; }
        public int Level { get; set; }
    }
}
