using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserManagement.UserAPI.Models;
using UserManagement.UserAPI.ViewModels;

namespace UserManagement.UserAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private UserManagementContext db;
        private IMapper mapper;

        public UserRepository(UserManagementContext _db, IMapper _mapper)
        {
            db = _db;
            mapper = _mapper;
        }

        public int AddClient(ClientViewModel clientViewModel)
        {
            Clients client = mapper.Map<Clients>(clientViewModel);
            db.Clients.Add(client);

            db.SaveChanges();

            return client.ClientId;
        }

        public int AddManager(ManagerViewModel managerViewModel)
        {
            Managers manager = mapper.Map<Managers>(managerViewModel);
            db.Managers.Add(manager);

            db.SaveChanges();

            return manager.ManagerId;
        }

        public int DeleteClient(int clientId)
        {
            var client = db.Clients.FirstOrDefault(c => c.ClientId == clientId);
            if(client != null)
            {
                return DeleteUser(client.UserId);
            }

            return 0;
        }

        public int DeleteManager(int managerId)
        {
            var manager = db.Managers.FirstOrDefault(c => c.ManagerId == managerId);
            if (manager != null)
            {
                return DeleteUser(manager.UserId);
            }

            return 0;
        }

        private int DeleteUser(int userId)
        {
            var user = db.Users.FirstOrDefault(u => u.UserId == userId);
            if(user != null)
            {
                db.Users.Remove(user);
                return db.SaveChanges();
            }

            return 0;
        }

        public IEnumerable<ClientViewModel> GetAllClients()
        {
            // Mapping doesn't work here, weird.
            return from c in db.Clients.Include(p => p.User).Include(p => p.Manager).ThenInclude(p => p.User)
                   select mapper.Map<ClientViewModel>(new Tuple<Clients, Users>(c, c.User));

            //var clients = (from c in db.Clients.Include(p => p.User).Include(p => p.Manager).ThenInclude(p => p.User)
            //               select c).ToArray();

            //foreach (var c in clients)
            //{
            //    yield return mapper.Map<ClientViewModel>(new Tuple<Clients, Users>(c, c.User));
            //}
        }

        public IEnumerable<ManagerViewModel> GetAllManagers()
        {
            var managerModels = (from m in db.Managers.Include(c => c.Clients).Include("Clients.User")
                                 join u in db.Users on m.UserId equals u.UserId
                                 select new Tuple<Managers, Users>(m, u)).ToArray();

            List<ManagerViewModel> lstManagers = new List<ManagerViewModel>();
            foreach(var m in managerModels)
            {
                var managerViewModel = mapper.Map<ManagerViewModel>(m);
                if(m.Item1.Clients != null)
                {
                    managerViewModel.Clients = new Collection<ClientViewModel>();
                    foreach (var c in m.Item1.Clients)
                    {
                        managerViewModel.Clients.Add(mapper.Map<ClientViewModel>(new Tuple<Clients, Users>(c, c.User)));
                    }
                }

                lstManagers.Add(managerViewModel);
            }

            return lstManagers;
        }

        public ClientViewModel GetClient(int clientId)
        {
            return (from c in db.Clients.Include(c => c.User).Include(c => c.Manager).Include(c => c.Manager.User)
                               where c.ClientId == clientId
                               select mapper.Map<ClientViewModel>(new Tuple<Clients, Users>(c, c.User))).FirstOrDefault();
        }

        public IEnumerable<ClientViewModel> GetClientsByManagerUsername(string userName)
        {
            var query = from m in db.Managers.Include(p => p.User)
                        join c in db.Clients on m.ManagerId equals c.ManagerId
                        join u in db.Users on c.UserId equals u.UserId
                        where m.User.UserName == userName
                        select mapper.Map<ClientViewModel>(new Tuple<Clients, Users>(c, u));

            return query.ToArray();
        }

        public int UpdateClient(ClientViewModel client)
        {
            var clientModel = (from c in db.Clients
                               where c.ClientId == client.ClientId select c).Include(c => c.User).FirstOrDefault();

            if (clientModel != null)
            {
                db.Users.Attach(clientModel.User);
                mapper.Map(client, clientModel);
                
                return db.SaveChanges();
            }

            return 0;
        }

        public bool UpdateManager(ManagerViewModel manager)
        {
            throw new NotImplementedException();
        }

        public ManagerViewModel GetManager(int managerId)
        {
            return (from m in db.Managers.Include(c => c.User)
                    where m.ManagerId == managerId
                    select mapper.Map<ManagerViewModel>(new Tuple<Managers, Users>(m, m.User))).FirstOrDefault();
        }
    }
}
