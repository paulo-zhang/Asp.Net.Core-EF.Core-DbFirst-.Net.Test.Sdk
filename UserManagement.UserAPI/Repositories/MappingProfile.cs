using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.UserAPI.Models;
using UserManagement.UserAPI.ViewModels;

namespace UserManagement.UserAPI.Repositories
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ClientViewModel, Clients>()
                    .ForMember(dest => dest.User,
                    opts => opts.MapFrom(src => new Users()
                    {
                        UserName = src.UserName,
                        FirstName = src.FirstName,
                        LastName = src.LastName,
                        Alias = src.Alias,
                        Email = src.Email
                    }));

            CreateMap<ManagerViewModel, Managers>()
                    .ForMember(dest => dest.User,
                    opts => opts.MapFrom(src => new Users()
                    {
                        UserName = src.UserName,
                        FirstName = src.FirstName,
                        LastName = src.LastName,
                        Alias = src.Alias,
                        Email = src.Email
                    }));

            CreateMap<Tuple<Managers, Users>, ManagerViewModel>()
                .ForMember(dest => dest.Alias, opts => opts.MapFrom(src => src.Item2.Alias))
                .ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.Item2.Email))
                .ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.Item2.FirstName))
                .ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.Item2.LastName))
                .ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.Item2.UserName))
                .ForMember(dest => dest.ManagerId, opts => opts.MapFrom(src => src.Item1.ManagerId))
                .ForMember(dest => dest.Position, opts => opts.MapFrom(src => (Positions)Enum.Parse(typeof(Positions),src.Item1.Position)));

            CreateMap<Tuple<Clients, Users>, ClientViewModel>()
                .ForMember(dest => dest.Alias, opts => opts.MapFrom(src => src.Item2.Alias))
                .ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.Item2.Email))
                .ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.Item2.FirstName))
                .ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.Item2.LastName))
                .ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.Item2.UserName))
                .ForMember(dest => dest.Level, opts => opts.MapFrom(src => src.Item1.Level))
                .ForMember(dest => dest.ClientId, opts => opts.MapFrom(src => src.Item1.ClientId))
                .ForMember(dest => dest.Manager, opts => opts.MapFrom(
                    src => src.Item1.Manager== null?null:new ManagerViewModel()
                    {
                        Alias = src.Item1.Manager.User.Alias,
                        Email = src.Item1.Manager.User.Email,
                        FirstName = src.Item1.Manager.User.FirstName,
                        LastName = src.Item1.Manager.User.LastName,
                        ManagerId = src.Item1.ManagerId.Value,
                        Position = (Positions)Enum.Parse(typeof(Positions), src.Item1.Manager.Position),
                        UserName = src.Item1.Manager.User.UserName
                    }));
        }
    }
}
