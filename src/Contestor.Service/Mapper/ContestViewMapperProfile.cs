using AutoMapper;
using Contestor.Proto.Data;

namespace Contestor.Proto.Mapper
{
    public class ContestViewMapperProfile : Profile
    {
        public ContestViewMapperProfile()
        {
            CreateMap<ContestModel, ContestManageViewModel>();
            CreateMap<ContestManageViewModel, ContestModel>();

            CreateMap<WorkWithVotesModel, WorkForVoteViewModel>();
        }
    }
}
