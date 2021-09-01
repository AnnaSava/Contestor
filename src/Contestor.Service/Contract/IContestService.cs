using Contestor.Data.Contract;
using Contestor.Data.Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Service.Contract
{
    public interface IContestService
    {
        Task<ContestModel> Create(ContestModel model);

        Task Open(long contestId);

        Task OpenRegistration(long contestId);

        Task StartVoting(long contestId);

        Task WaitWinner(long contestId);

        Task SetFinishedStatus(long contestId);

        Task<ContestModel> GetOne(long id);

        Task<ContestManageViewModel> GetOneForManage(long id);

        Task<IEnumerable<ContestModel>> GetAll(int page, int count);

        Task<IEnumerable<ContestModel>> GetAllForNewParticipants(long visitorId, int page, int count);

        Task<IEnumerable<ContestModel>> GetAllForVoting();

        Task<IEnumerable<ContestModel>> GetTopForVoting(long visitorId);

        Task<Dictionary<string, string>> GetProcessesDictionary();

        Task<string> StartContestProcess(long contestId);

        Task RegisterParticipant(long contestId, long userId);

        Task<ParticipantModel> GetParticipant(long contestId, long userId);

        Task<int> GetParticipantsCount(long contestId);

        Task<int> GetParticipantsHavingWorkCount(long contestId);

        Task SendWork(WorkModel model);

        Task CompleteTask(CompletingTaskViewModel model);

        Task<IEnumerable<WorkForVoteViewModel>> GetAllWorks(long contestId, long visitorId);

        Task Vote(long voterId, long workId);

        Task<IEnumerable<WorkWithScoreModel>> GetTopVotedWorks(long contestId);
    }
}
