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

        Task<int> Login(LoginViewModel model);

        Task Logout();
     }
}
