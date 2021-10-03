using Contestor.Proto.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contestor.Proto
{
    public interface IContestService
    {
        Task<ContestModel> Create(ContestModel model);

        Task<ContestModel> Update(ContestModel model);

        Task SetDueDate(long contestId, string date);

        Task Open(long contestId);

        Task OpenRegistration(long contestId);

        Task CloseRegistration(long contestId);

        Task StartVoting(long contestId);

        Task WaitWinner(long contestId);

        Task SetFinishedStatus(long contestId);

        Task<ContestModel> GetOne(long id);

        Task<ContestModel> GetOne(long id, long visitorId);

        Task<ContestManageViewModel> GetOneForManage(long id);

        Task<IEnumerable<ContestModel>> GetAll(int page, int count);

        Task<IEnumerable<ContestModel>> GetAllByUser(long userId, int page, int count);

        Task<IEnumerable<ContestModel>> GetAllForNewParticipants(long visitorId, int page, int count);

        Task<IEnumerable<ContestModel>> GetAllForVoting();

        Task<IEnumerable<ContestModel>> GetTopForVoting(long visitorId);

        Task<Dictionary<string, string>> GetProcessesDictionary();

        Task<string> StartContestProcess(long contestId, string apiUrl);

        Task RegisterParticipant(long contestId, long userId);

        Task<ParticipantModel> GetParticipant(long contestId, long userId);

        Task<int> GetParticipantsCount(long contestId);

        Task<IEnumerable<ParticipantModel>> GetAllParticipants(long contestId);

        Task<int> GetParticipantsHavingWorkCount(long contestId);

        Task SendWork(WorkModel model);

        Task<bool> CompleteTask(CompletingTaskViewModel model);

        Task<IEnumerable<WorkModel>> GetAllWorks(long contestId);

        Task<IEnumerable<WorkForVoteViewModel>> GetAllWorks(long contestId, long visitorId);

        Task<IEnumerable<WorkModel>> GetWorksByUser(long userId, int page, int count);

        Task Vote(long voterId, long workId);

        Task<IEnumerable<WorkWithScoreModel>> GetTopVotedWorks(long contestId);
    }
}
