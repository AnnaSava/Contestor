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

        Task<IEnumerable<ContestModel>> GetAll(int page, int count);

        Task<IEnumerable<ContestModel>> GetAllForNewParticipants(long visitorId, int page, int count);

        Task<Dictionary<string, string>> GetProcessesDictionary();

        Task<string> StartContestProcess(long contestId);

        Task RegisterParticipant(long contestId, long userId);

        Task<ParticipantModel> GetParticipant(long contestId, long userId);

        Task SendWork(WorkModel model);
    }
}
