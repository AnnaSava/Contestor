using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contestor.Proto.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contestor.Proto.Data.Services
{
    public class ContestService : BaseService, IContestService
    {
        public ContestService(ContestContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {

        }

        public async Task<ContestModel> Create(ContestModel model)
        {
            var entity = _mapper.Map<Contest>(model);

            _dbContext.Contests.Add(entity);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<ContestModel>(entity);
        }

        public async Task<ContestModel> Update(ContestModel model)
        {
            var entity = await _dbContext.Contests.FirstOrDefaultAsync(m => m.Id == model.Id);
            if (entity == null) throw new Exception($"Entity {model.Id} not found");

            _mapper.Map(model, entity);

            entity.Status = ContestStatus.Draft;

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

        public async Task<long> SetDueDate(long contestId, DateTime? date)
        {
            var entity = await _dbContext.Contests.FirstOrDefaultAsync(m => m.Id == contestId);
            if (entity == null) throw new Exception($"Entity {contestId} not found");

            entity.CurStageEndDate = date;
            await _dbContext.SaveChangesAsync();

            await LogContest(entity.Id, nameof(SetDueDate), date.HasValue ? date.Value.ToString() : null);

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

        public async Task<ContestModel> GetOne(long id, long visitorId)
        {
            var entity = await _dbContext.Contests
                .Where(m => m.Id == id)
                .Include(m => m.Participants)
                .Include(m => m.Works)
                .FirstOrDefaultAsync();

            var model = _mapper.Map<ContestModel>(entity);

            if (entity.Participants.Select(m => m.UserId).Contains(visitorId))
            {
                model.VisitorIsParticipant = true;
            }

            model.VisitorWorksCount = entity.Works.Where(m => m.ParticipantId == visitorId).Count();

            return model;
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

        public async Task<IEnumerable<ContestModel>> GetAllByUser(long userId, int page, int count)
        {
            return await _dbContext.Contests
                .Include(m => m.Participants)
                .Where(m => m.Participants.Select(m => m.UserId).Contains(userId))
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

        public async Task<IEnumerable<ContestModel>> GetAllForVoting(int count = 100)
        {
            // TODO фильтровать жюри. Пока голосовать могут все.

            return await _dbContext.Contests
                .Where(m => m.Status == ContestStatus.Voting)
                .OrderBy(m => m.Id)
                .Take(count)
                .AsNoTracking()
                .ProjectTo<ContestModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<ContestModel>> GetTopForVoting(long visitorId, int count = 3)
        {
            if (visitorId == 0) return await GetAllForVoting(count);

            // Не показываем те конкурсы, в которых пользователь уже проголосовал
            return await _dbContext.Contests.Include(m => m.Works).ThenInclude(m => m.Votes)
                .Where(m => m.Status == ContestStatus.Voting)
                .Where(m => m.Works.SelectMany(m => m.Votes).Select(m => m.VoterId).Contains(visitorId) == false)
                .OrderBy(m => m.Id)
                .Take(count)
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

        public async Task<IEnumerable<WorkModel>> GetWorksByUser(long userId, int page, int count)
        {
            return await _dbContext.Works
                .Where(m => m.ParticipantId == userId)
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

        public async Task<int> GetWorksHavingVotesCount(long contestId)
        {
            return await _dbContext.Works
                .Where(m => m.ContestId == contestId && m.VotesSum > 0)
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

        public async Task<IEnumerable<WorkWithVotesModel>> GetAllWorksWithVotes(long contestId)
        {
            return await _dbContext.Works
                 .Where(m => m.ContestId == contestId)
                 .Include(m => m.Votes)
                 .ProjectTo<WorkWithVotesModel>(_mapper.ConfigurationProvider)
                 .ToListAsync();
        }

        public async Task Vote(long voterId, long workId, int points = 1)
        {
            // TODO решить, в сервисе какого уровня должна быть эта проверка
            var work = await _dbContext.Works.Include(m => m.Votes).FirstOrDefaultAsync(m => m.Id == workId);
            if (work.ParticipantId == voterId) throw new Exception("you can't vote for yourself");

            // TODO и эта. В отличие от первого случая здесь будет эксепшен в БД при попытке добавить уже существующий PK
            if (work.Votes.Any(m => m.VoterId == voterId)) throw new Exception("you have already voted");

            var vote = new Vote
            {
                VoterId = voterId,
                WorkId = workId,
                Points = points
            };

            _dbContext.Votes.Add(vote);

            work.VotesSum += points;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<WorkWithScoreModel>> GetTopVotedWorks(long contestId, int worksCount = 3)
        {
            //var wv = await _dbContext.Votes
            //      .Join(_dbContext.Works,
            //      v => v.WorkId,
            //      w => w.Id,
            //      (v, w) => new { Work = w, Vote = v })
            //      .Where(m => m.Work.ContestId == contestId)
            //      .ToListAsync();

            //var res = wv.GroupBy(m => m.Work)
            //    .Select(m => new WorkWithScoreModel { Work = _mapper.Map<WorkModel>(m.Key), Score = m.Sum(s => s.Vote.Points) })
            //    .OrderByDescending(m => m.Score)
            //    .Take(worksCount);

            //return res;

            return await _dbContext.Works
                      .Where(m => m.ContestId == contestId)
                      .OrderByDescending(m => m.VotesSum)
                      .Take(worksCount)
                      .Select(m => new WorkWithScoreModel
                      { Work = _mapper.Map<WorkModel>(m), Score = m.VotesSum })
                      .ToListAsync();
        }

        public async Task SetWinnerPlaces(long contestId)
        {
            var lowestPlace = 3;
            var currentPlace = 1;
            var currentVotesSum = 0;
            var watched = 0;

            while (currentPlace < lowestPlace)
            {
                var work = await _dbContext.Works
                    .Where(m => m.ContestId == contestId && m.VotesSum > 0)
                    .OrderByDescending(m => m.VotesSum)
                    .Skip(watched)
                    .FirstOrDefaultAsync();

                if (work == null) break;

                // Если рассматриваем первую работу
                if (watched == 0)
                {
                    currentVotesSum = work.VotesSum;
                }

                if (work.VotesSum < currentVotesSum)
                {
                    currentVotesSum = work.VotesSum;
                    currentPlace++;
                }

                work.Place = currentPlace;
                watched++;
            }

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
