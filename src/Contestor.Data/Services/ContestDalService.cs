using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contestor.Data.Contract.Interfaces;
using Contestor.Data.Contract.Models;
using Contestor.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Data.Services
{
    public class ContestDalService : BaseService, IContestDalService
    {
        public ContestDalService(ContestDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {

        }

        public async Task<ContestModel> Create(ContestModel model)
        {
            var entity = _mapper.Map<Contest>(model);

            _dbContext.Contests.Add(entity);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<ContestModel>(entity);
        }

        public async Task<ContestModel> GetOne(long id)
        {
            var entity = await _dbContext.Contests
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();

            return _mapper.Map<ContestModel>(entity);
        }

        public async Task<IEnumerable<ContestModel>> GetAll(int page, int count)
        {
            return await _dbContext.Contests
                .AsNoTracking()
                .OrderBy(m => m.Id)
                .Skip((page - 1) * count)
                .Take(count)
                .ProjectTo<ContestModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<ParticipantModel> CreateParticipant(ParticipantModel model)
        {
            var entity = _mapper.Map<Participant>(model);

            _dbContext.Participants.Add(entity);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<ParticipantModel>(entity);
        }

        public async Task<IEnumerable<ParticipantModel>> GetAllParticipants(long contestId, int page, int count)
        {
            return await _dbContext.Participants
                .Where(m => m.ContestId == contestId)
                .AsNoTracking()
                .OrderBy(m => m.UserId)
                .Skip((page - 1) * count)
                .Take(count)
                .ProjectTo<ParticipantModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<WorkModel> CreateWork(WorkModel model)
        {
            var entity = _mapper.Map<Contest>(model);

            _dbContext.Contests.Add(entity);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<WorkModel>(entity);
        }

        public async Task<IEnumerable<WorkModel>> GetAllWorks(long contestId, int page, int count)
        {
            return await _dbContext.Works
                .Where(m => m.ContestId == contestId)
                .AsNoTracking()
                .OrderBy(m => m.Id)
                .Skip((page - 1) * count)
                .Take(count)
                .ProjectTo<WorkModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
