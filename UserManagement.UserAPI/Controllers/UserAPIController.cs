using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UserManagement.UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAPIController : ControllerBase
    {
        // GET: api/Users/GetAllManagers
        /// <summary>
        /// Retreive all Managers with their associated clients
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllManagers")]
        public ActionResult<IEnumerable<ManagerViewModel>> GetAllManagers()
        {
            return new ActionResult<IEnumerable<ManagerViewModel>>(userRepository.GetAllManagers());
        }

        // GET: api/Users/GetClient/{id}
        [HttpGet("GetClient/{id}")]
        public ActionResult<ClientViewModel> GetClient(int id)
        {
            return new ActionResult<ClientViewModel>(userRepository.GetClient(id));
        }

        // GET: api/Users/GetManager/{id}
        [HttpGet("GetManager/{id}")]
        public ActionResult<ManagerViewModel> GetManager(int id)
        {
            return new ActionResult<ManagerViewModel>(userRepository.GetManager(id));
        }

        // GET: api/Users/GetClient
        /// <summary>
        /// - Retreive all Clients including their Manager
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllClients")]
        public ActionResult<IEnumerable<ClientViewModel>> GetAllClients()
        {
            return new ActionResult<IEnumerable<ClientViewModel>>(userRepository.GetAllClients());
        }

        // HTTP test tool: {"Level":2,"UserName":"client1","Email":"client1@gmail.com","Alias":"Alias1","FirstName":"Sally","LastName":"Jhonson"}
        // POST: api/Users
        [HttpPost]
        [Route("AddClient")]
        public ActionResult<int> AddClient([FromBody] ClientViewModel client)
        {
            this.ValidateViewModel(client);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var clientId = userRepository.AddClient(client);

            if (clientId > 0)
            {
                return clientId;
            }

            return BadRequest();
        }

        // HTTP test tool: {"Level":2,"UserName":"manager1","Email":"client1@gmail.com","Alias":"Alias1","FirstName":"Sally","LastName":"Jhonson", "Position": "Junior"}
        // POST: api/Users
        [HttpPost]
        [Route("AddManager")]
        public ActionResult<int> AddManager([FromBody] ManagerViewModel manager)
        {
            this.ValidateViewModel(manager);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var managerId = userRepository.AddManager(manager);

            if (managerId > 0)
            {
                return managerId;
            }

            return BadRequest();
        }

        // PUT: api/Users/UpdateClient
        [HttpPost]
        [Route("UpdateClient")]
        public ActionResult<int> UpdateClient(ClientViewModel client)
        {
            return userRepository.UpdateClient(client);
        }

        // DELETE: api/Users/DeleteClient/5
        [HttpDelete("DeleteClient/{id}")]
        public ActionResult<int> DeleteClient(int id)
        {
            return userRepository.DeleteClient(id);
        }

        // DELETE: api/Users/DeleteManager/5
        [HttpDelete("DeleteManager/{id}")]
        public ActionResult<int> DeleteManager(int id)
        {
            return userRepository.DeleteManager(id);
        }

        /// <summary>
        /// - For a specified Manager username, retreive a list of its clients
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetClientsByManagerUserName")]
        public ActionResult<IEnumerable<ClientViewModel>> GetClientsByManagerUserName(string userName)
        {
            return new ActionResult<IEnumerable<ClientViewModel>>(userRepository.GetClientsByManagerUsername(userName));
        }
    }
}
