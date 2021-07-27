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
            CreateMap<ContestModel, Contest>()
                .ForMember(x => x.Status, y => y.MapFrom(s => string.IsNullOrEmpty(s.Status) ? "draft" : s.Status))
                .ForMember(x => x.MaxWorksCount, y => y.MapFrom(s => s.MaxWorksCount == 0 ? 1 : s.MaxWorksCount));

            CreateMap<Participant, ParticipantModel>();
            CreateMap<ParticipantModel, Participant>();

            CreateMap<Work, WorkModel>();
            CreateMap<WorkModel, Work>();
        }
    }
}
