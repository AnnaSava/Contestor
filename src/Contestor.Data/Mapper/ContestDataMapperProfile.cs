﻿using AutoMapper;
using Contestor.Proto.Data.Entities;

namespace Contestor.Proto.Data.Mapper
{
    public class ContestDataMapperProfile : Profile
    {
        public ContestDataMapperProfile()
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

            CreateMap<Work, WorkWithVotesModel>();
            CreateMap<WorkWithVotesModel, Work>();

            CreateMap<Vote, VoteModel>();
            CreateMap<VoteModel, Vote>();
        }
    }
}
