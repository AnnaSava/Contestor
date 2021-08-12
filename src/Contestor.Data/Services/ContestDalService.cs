using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contestor.Data.Contract;
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

        public async Task<long> SetStatus(long contestId, string status)
        {
            var entity = await _dbContext.Contests.FirstOrDefaultAsync(m => m.Id == contestId);
            if (entity == null) throw new Exception($"Entity {contestId} not found");

            entity.Status = status;
            await _dbContext.SaveChangesAsync();

            await LogContest(entity.Id, nameof(SetStatus), status);

            return entity.Id;
        }

        // TODO объединить с другими методами?
        public async Task<long> SetAutoRegEnabled(long contestId, bool autoRegEnabled)
        {
            var entity = await _dbContext.Contests.FirstOrDefaultAsync(m => m.Id == contestId);
            if (entity == null) throw new Exception($"Entity {contestId} not found");

            entity.AutoRegEnabled = autoRegEnabled;
            await _dbContext.SaveChangesAsync();

            await LogContest(entity.Id, nameof(SetAutoRegEnabled), autoRegEnabled.ToString());

            return entity.Id;
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
                .OrderByDescending(m => m.Id)
                .Skip((page - 1) * count)
                .Take(count)
                .ProjectTo<ContestModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<ContestModel>> GetAllByStatus(string status, int page, int count)
        {
            return await _dbContext.Contests
                .Where(m => m.Status == status)
                .AsNoTracking()
                .OrderByDescending(m => m.Id)
                .Skip((page - 1) * count)
                .Take(count)
                .ProjectTo<ContestModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<ContestModel>> GetAllForNewParticipants(long visitorId, int page, int count)
        {
            if (visitorId == 0) return await _dbContext.Contests
                  .Where(m => m.Status == ContestStatus.RegistrationOpen || (m.Status == ContestStatus.Open && m.AutoRegEnabled))
                  .AsNoTracking()
                  .OrderByDescending(m => m.Id)
                  .Skip((page - 1) * count)
                  .Take(count)
                  .ProjectTo<ContestModel>(_mapper.ConfigurationProvider)
                  .ToListAsync();

            var visitorRegDoneContests = await _dbContext.Contests
                .Where(m => m.Status == ContestStatus.RegistrationOpen || m.Status == ContestStatus.Open)
                .Join(_dbContext.Participants.Where(m => m.UserId == visitorId),
                c => c.Id,
                p => p.ContestId,
                (c, p) => new { c.Id, p.WorksCount }).ToListAsync();

            var visitorRegDoneContestIds = visitorRegDoneContests.Select(m => m.Id);

            var contests = await _dbContext.Contests
                  .Where(m => m.Status == ContestStatus.RegistrationOpen || (m.Status == ContestStatus.Open && (m.AutoRegEnabled || visitorRegDoneContestIds.Contains(m.Id))))
                  .AsNoTracking()
                  .OrderByDescending(m => m.Id)
                  .Skip((page - 1) * count)
                  .Take(count)
                  .ProjectTo<ContestModel>(_mapper.ConfigurationProvider)
                  .ToListAsync();

            foreach (var contest in contests)
            {
                var visitorContest = visitorRegDoneContests.Where(m => m.Id == contest.Id).FirstOrDefault();
                if (visitorContest != null)
                {
                    contest.VisitorIsParticipant = true;
                    contest.VisitorWorksCount = visitorContest.WorksCount;
                }
            }
            return contests;
        }

        public async Task<IEnumerable<ContestModel>> GetAllForVoting()
        {
            // TODO фильтровать жюри. Пока голосовать могут все.

            return await _dbContext.Contests
                .Where(m => m.Status == ContestStatus.Voting)
                .AsNoTracking()
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

        public async Task RegisterParticipant(long contestId, long userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(m=>m.Id == userId);

            var participant = new Participant
            {
                UserId = userId,
                ContestId = contestId,
                DisplayName = user.UserName
            };

            _dbContext.Participants.Add(participant);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ParticipantModel> GetParticipant(long contestId, long userId)
        {
            var entity = await _dbContext.Participants.FirstOrDefaultAsync(m => m.ContestId == contestId && m.UserId == userId);

            return _mapper.Map<ParticipantModel>(entity);
        }

        public async Task<int> GetParticipantsCount(long contestId)
        {
            return await _dbContext.Participants.CountAsync(m => m.ContestId == contestId);
        }

        public async Task<int> GetParticipantsHavingWorkCount(long contestId)
        {
            return await _dbContext.Works
                .Where(m => m.ContestId == contestId)
                .Select(m => m.ParticipantId)
                .Distinct()
                .CountAsync();
        }

        public async Task SendWork(WorkModel model)
        {
            var entity = _mapper.Map<Work>(model);

            _dbContext.Works.Add(entity);

            var participant = await _dbContext.Participants.FirstOrDefaultAsync(m => m.ContestId == model.ContestId && m.UserId == model.ParticipantId);

            if (participant == null) throw new Exception($"Participant {model.ParticipantId} not found for contest {model.ContestId}");
            participant.WorksCount++;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<WorkModel>> GetAllWorks(long contestId)
        {
            return await _dbContext.Works.Where(m => m.ContestId == contestId)
                .AsNoTracking()
                .ProjectTo<WorkModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task Vote(long voterId, long workId, int points = 1)
        {
            var vote = new Vote
            {
                VoterId = voterId,
                WorkId = workId,
                Points = points
            };

            _dbContext.Votes.Add(vote);
            await _dbContext.SaveChangesAsync();
        }

        private async Task LogContest(long contestId, string action, string value = null, string message = null)
        {
            var log = new ContestLog
            {
                ContestId = contestId,
                Action = action,
                Value = value,
                Message = message,
                DateTime = DateTime.Now
            };

            _dbContext.ContestLogs.Add(log);
            await _dbContext.SaveChangesAsync();
        }
    }
}
