using AutoMapper;
using Contestor.Proto.Data;

namespace Contestor.Proto.Mapper
{
    public class ContestMapperProfile : Profile
    {
        public ContestMapperProfile()
        {
            CreateMap<ContestModel, ContestManageViewModel>();
            CreateMap<ContestManageViewModel, ContestModel>();

            CreateMap<WorkWithVotesModel, WorkForVoteViewModel>();
        }
    }
}
