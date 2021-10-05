using System.Threading.Tasks;

namespace Contestor.Proto.Data
{
    public interface IUserService
    {
        Task<long> Register(UserModel model, string password);

        Task Login(string userName, string password);

        Task Logout();
    }
}
