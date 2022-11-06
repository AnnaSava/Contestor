using AutoMapper;
using Contestor.Proto.Data;
using System.Threading.Tasks;

namespace Contestor.Proto.Services
{
    public class UserViewService : IUserViewService
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserViewService(IUserService userDalService, IMapper mapper)
        {
            _userService = userDalService;
            _mapper = mapper;
        }

        public async Task<long> Register(RegisterViewModel model)
        {
            var user = new UserModel { UserName = model.UserName };
            return await _userService.Register(user, model.Password);
        }

        public async Task<int> Login(LoginViewModel model)
        {
          return  await _userService.Login(model.UserName, model.Password);
        }

        public async Task<UserModel> Auth(LoginViewModel model)
        {
            return await _userService.Auth(model.UserName, model.Password);
        }

        public async Task Logout()
        {
            await _userService.Logout();
        }
    }
}
