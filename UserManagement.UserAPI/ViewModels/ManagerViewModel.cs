using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.UserAPI.ViewModels
{
    public class ManagerViewModel : UserViewModel
    {
        public int ManagerId { get; set; }
        public Positions Position { get; set; }
        public ICollection<ClientViewModel> Clients { get; set; }
    }

    public enum Positions
    {
        Junior,
        Senior
    }
}
