using Contestor.Data.Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Data.Contract
{
   public interface IUserDalService
    {
        Task<long> Register(UserModel model, string password);

        Task Login(string userName, string password);

        Task Logout();
    }
}
