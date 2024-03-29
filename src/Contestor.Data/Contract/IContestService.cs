﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Proto.Data
{
    public interface IContestService
    {
        Task<ContestModel> Create(ContestModel model);

        Task<ContestModel> Update(ContestModel model);

        Task<long> SetStatus(long contestId, string status);

        Task<long> SetDueDate(long contestId, DateTime? date);

        Task<long> SetAutoRegEnabled(long contestId, bool autoRegEnabled);

        Task<ContestModel> GetOne(long id);

        Task<ContestModel> GetOne(long id, long visitorId);

        Task<IEnumerable<ContestModel>> GetAll(int page, int count);

        Task<IEnumerable<ContestModel>> GetAllByStatus(string status, int page, int count);

        Task<IEnumerable<ContestModel>> GetAllByUser(long userId, int page, int count);

        Task<IEnumerable<ContestModel>> GetAllForNewParticipants(long visitorId, int page, int count);

        Task<IEnumerable<ContestModel>> GetAllForVoting(int count = 100);

        Task<IEnumerable<ContestModel>> GetTopForVoting(long visitorId, int count = 3);

        Task<ParticipantModel> CreateParticipant(ParticipantModel model);

        Task<IEnumerable<ParticipantModel>> GetAllParticipants(long contestId, int page, int count);

        Task<int> GetParticipantsCount(long contestId);

        Task<int> GetParticipantsHavingWorkCount(long contestId);

        Task<int> GetWorksHavingVotesCount(long contestId);

        Task<WorkModel> CreateWork(WorkModel model);

        Task<IEnumerable<WorkModel>> GetAllWorks(long contestId, int page, int count);

        Task<IEnumerable<WorkModel>> GetAllWorks(long contestId);

        Task<IEnumerable<WorkModel>> GetWorksByUser(long userId, int page, int count);

        Task<IEnumerable<WorkWithVotesModel>> GetAllWorksWithVotes(long contestId);

        Task RegisterParticipant(long contestId, long userId);

        Task<ParticipantModel> GetParticipant(long contestId, long userId);

        Task SendWork(WorkModel model);

        Task Vote(long voterId, long workId, int points = 1);

        Task<IEnumerable<WorkWithScoreModel>> GetTopVotedWorks(long contestId, int worksCount = 3);

        Task SetWinnerPlaces(long contestId);
    }
}
