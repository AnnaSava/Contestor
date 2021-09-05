using AutoMapper;
using Contestor.Data.Contract;
using Contestor.Data.Contract.Models;
using Contestor.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Service.Mapper
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
