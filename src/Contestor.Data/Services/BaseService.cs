using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Proto.Data.Services
{
    public abstract class BaseService
    {
        protected readonly ContestContext _dbContext;
        protected readonly IMapper _mapper;

        public BaseService(ContestContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
    }
}
