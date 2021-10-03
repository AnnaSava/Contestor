using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contestor.Proto
{
    public interface IUserViewService
    {
        Task<long> Register(RegisterViewModel model);

        Task Login(LoginViewModel model);

        Task Logout();
     }
}
