using AutoMapper;
using Contestor.Data.Contract.Models;
using Contestor.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Data.Mapper
{
    public class ContestMapperProfile : Profile
    {
        public ContestMapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<UserModel, User>();

            CreateMap<Role, RoleModel>();
            CreateMap<RoleModel, Role>();

            CreateMap<Contest, ContestModel>();
            CreateMap<ContestModel, Contest>();

            CreateMap<Participant, ParticipantModel>();
            CreateMap<ParticipantModel, Participant>();

            CreateMap<Work, WorkModel>();
            CreateMap<WorkModel, Work>();
        }
    }
}
