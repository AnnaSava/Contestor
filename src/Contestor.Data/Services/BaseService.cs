using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Data.Services
{
    public abstract class BaseService
    {
        protected readonly ContestDbContext _dbContext;
        protected readonly IMapper _mapper;

        public BaseService(ContestDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
    }
}
