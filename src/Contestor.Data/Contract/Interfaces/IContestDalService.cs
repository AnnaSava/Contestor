using Contestor.Data.Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Data.Contract.Interfaces
{
    public interface IContestDalService
    {
        Task<ContestModel> Create(ContestModel model);

        Task<ContestModel> GetOne(long id);

        Task<IEnumerable<ContestModel>> GetAll(int page, int count);

        Task<ParticipantModel> CreateParticipant(ParticipantModel model);

        Task<IEnumerable<ParticipantModel>> GetAllParticipants(long contestId, int page, int count);

        Task<WorkModel> CreateWork(WorkModel model);

        Task<IEnumerable<WorkModel>> GetAllWorks(long contestId, int page, int count);
    }
}
