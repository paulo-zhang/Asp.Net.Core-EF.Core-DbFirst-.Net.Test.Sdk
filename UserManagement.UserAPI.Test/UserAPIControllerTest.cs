using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.UserAPI.Controllers;
using UserManagement.UserAPI.Models;
using UserManagement.UserAPI.Repositories;
using UserManagement.UserAPI.ViewModels;
using Xunit;

namespace UserManagement.UserAPI.Test
{
    public class UserAPIControllerTest
    {
        private UserAPIController usersController;
        public UserAPIControllerTest()
        {
            // Use database to test here, we could mock UserManagementContext if we only need to test controller.
            var optionsBuilder = new DbContextOptionsBuilder<UserManagementContext>();
            optionsBuilder.UseSqlServer(@"Server=localhost\SQLExpress;Database=UserManagement;UID=sa;PWD=Abcd1234;");
            UserManagementContext domainContext = new UserManagementContext(optionsBuilder.Options);

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();

            usersController = new UserAPIController(new UserRepository(domainContext, mapper));
        }

        [Fact]
        public void AddClient_Returns_OK()
        {
            var client = AddClient();
            Assert.True(client.ClientId > 0);
        }

        private ClientViewModel AddClient(int managerId = 0)
        {
            // Arrangge
            string timeStamp = DateTime.Now.ToString("yyyyMMddhhmmssfff");
            ClientViewModel client = new ClientViewModel()
            {
                UserName = "client" + timeStamp,
                FirstName = "Sally",
                LastName = "Jhonson",
                Alias = "Alias" + timeStamp,
                Email = string.Format("client{0}@gmail.com", timeStamp),
                Level = new Random().Next(1, 5),
                Manager = managerId == 0 ? null : new ManagerViewModel() { ManagerId = managerId }
            };

            //Act
            var result = usersController.AddClient(client);
            client.ClientId = result.Value;

            return client;
        }

        [Fact]
        public void AddClient_WrongEmail()
        {
            // Arrangge
            ClientViewModel client = new ClientViewModel()
            {
                UserName = "client1",
                FirstName = "Sally",
                LastName = "Jhonson",
                Alias = "Alias",
                Email = "client1#gmail.com",
                Level = new Random().Next(1, 5)
            };

            //Act
            var result = usersController.AddClient(client);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void AddManager_Returns_OK()
        {
            var manager = AddManager();
            Assert.True(manager.ManagerId > 0);
        }

        private ManagerViewModel AddManager()
        {
            // Arrangge
            string timeStamp = DateTime.Now.ToString("yyyyMMddhhmmssfff");
            ManagerViewModel manager = new ManagerViewModel()
            {
                UserName = "manager" + timeStamp,
                FirstName = "Sally",
                LastName = "Jhonson",
                Alias = "Alias" + timeStamp,
                Email = string.Format("manager{0}@gmail.com", timeStamp),
                Position = Positions.Junior
            };

            //Act
            var result = usersController.AddManager(manager);
            manager.ManagerId = result.Value;

            return manager;
        }

        [Fact]
        public void DeleteClient_OK()
        {
            var client = AddClient();
            var result = usersController.DeleteClient(client.ClientId);
            Assert.True(result.Value > 0);

            var queryResult = usersController.GetClient(client.ClientId);
            Assert.Null(queryResult.Value);
        }

        [Fact]
        public void DeleteManager_OK()
        {
            var manager = AddManager();
            var result = usersController.DeleteManager(manager.ManagerId);
            Assert.True(result.Value > 0);

            var queryResult = usersController.GetManager(manager.ManagerId);
            Assert.Null(queryResult.Value);
        }

        [Fact]
        public void UpdateClient_OK()
        {
            ClientViewModel client = AddClient();

            client.Alias += "_changed";
            client.Email = "changed_" + client.Email;
            client.FirstName += "_changed";
            client.LastName += "_changed";
            client.Level = new Random().Next(5, 10);
            client.UserName += "_changed";

            int ret = usersController.UpdateClient(client).Value;
            Assert.True(ret > 0);

            var result = usersController.GetClient(client.ClientId);
            ClientViewModel clientChanged = result.Value;

            Assert.Equal(client.Alias, clientChanged.Alias);
            Assert.Equal(client.Email, clientChanged.Email);
            Assert.Equal(client.FirstName, clientChanged.FirstName);
            Assert.Equal(client.LastName, clientChanged.LastName);
            Assert.Equal(client.Level, clientChanged.Level);
            Assert.Equal(client.UserName, clientChanged.UserName);
        }

        /// <summary>
        /// Retreive all Managers with their associated clients
        /// </summary>
        [Fact]
        public void GetAllManagers_OK()
        {
            var result = usersController.GetAllManagers();
            IEnumerable<ManagerViewModel> managers = result.Value as IEnumerable<ManagerViewModel>;

            Assert.True(managers.Count() > 0);
        }

        [Fact]
        public void AddManagerAndChildrenClients_OK()
        {
            var manager = AddManager();

            // Insert related clients.
            var client1 = AddClient(manager.ManagerId);
            var client2 = AddClient(manager.ManagerId);
            var client3 = AddClient(manager.ManagerId);
            var client4 = AddClient(manager.ManagerId);

            Assert.Equal(manager.ManagerId, client1.Manager.ManagerId);
            Assert.Equal(manager.ManagerId, client2.Manager.ManagerId);
            Assert.Equal(manager.ManagerId, client3.Manager.ManagerId);
            Assert.Equal(manager.ManagerId, client4.Manager.ManagerId);
        }

        /// <summary>
        /// - For a specified Manager username, retreive a list of its clients
        /// </summary>
        [Fact]
        public void GetManagerByUserName_OK()
        {
            var manager = AddManager();

            // Insert related clients.
            var client1 = AddClient(manager.ManagerId);
            var client2 = AddClient(manager.ManagerId);
            var client3 = AddClient(manager.ManagerId);
            var client4 = AddClient(manager.ManagerId);

            var clients = usersController.GetClientsByManagerUserName(manager.UserName).Value;

            Assert.Equal(4, clients.Count());
            Assert.Equal(client1.ClientId, clients.ElementAt(0).ClientId);
            Assert.Equal(client2.ClientId, clients.ElementAt(1).ClientId);
            Assert.Equal(client3.ClientId, clients.ElementAt(2).ClientId);
            Assert.Equal(client4.ClientId, clients.ElementAt(3).ClientId);

            Assert.Equal(manager.ManagerId, clients.ElementAt(0).Manager.ManagerId);
            Assert.Equal(manager.ManagerId, clients.ElementAt(1).Manager.ManagerId);
            Assert.Equal(manager.ManagerId, clients.ElementAt(2).Manager.ManagerId);
            Assert.Equal(manager.ManagerId, clients.ElementAt(3).Manager.ManagerId);
        }

        /// <summary>
        /// - Retreive all Clients including their Manager
        /// </summary>
        [Fact]
        public void GetAllClients_OK()
        {
            var clients = usersController.GetAllClients().Value;

            Assert.True(clients.Count() > 0);
        }
    }
}
