using System.Threading.Tasks;

namespace Contestor.Proto.Data
{
    public interface IUserDalService
    {
        Task<long> Register(UserModel model, string password);

        Task Login(string userName, string password);

        Task Logout();
    }
}
