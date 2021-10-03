using AutoMapper;
using Contestor.Proto.Data;
using System.Threading.Tasks;

namespace Contestor.Proto.Services
{
    public class UserService : IUserService
    {
        private readonly IUserDalService _userDalService;
        private readonly IMapper _mapper;

        public UserService(IUserDalService userDalService, IMapper mapper)
        {
            _userDalService = userDalService;
            _mapper = mapper;
        }

        public async Task<long> Register(RegisterViewModel model)
        {
            var user = new UserModel { UserName = model.UserName };
            return await _userDalService.Register(user, model.Password);
        }

        public async Task Login(LoginViewModel model)
        {
            await _userDalService.Login(model.UserName, model.Password);
        }

        public async Task Logout()
        {
            await _userDalService.Logout();
        }
    }
}
