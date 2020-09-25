using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.UserAPI.ViewModels;

namespace UserManagement.UserAPI.Repositories
{
    public interface IUserRepository
    {
        int AddManager(ManagerViewModel managerViewModel);
        bool UpdateManager(ManagerViewModel managerViewModel);
        int DeleteManager(int managerId);

        int AddClient(ClientViewModel clientViewModel);
        int UpdateClient(ClientViewModel clientViewModel);
        int DeleteClient(int clientId);
        ClientViewModel GetClient(int clientId);
        ManagerViewModel GetManager(int managerId);

        IEnumerable<ManagerViewModel> GetAllManagers();
        IEnumerable<ClientViewModel> GetAllClients();
        IEnumerable<ClientViewModel> GetClientsByManagerUsername(string userName);
    }
}
