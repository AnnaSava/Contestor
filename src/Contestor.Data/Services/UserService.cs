using AutoMapper;
using Contestor.Proto.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Contestor.Proto.Data.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public async Task<long> Register(UserModel model, string password)
        {
            var exist = await _userManager.FindByNameAsync(model.UserName);
            if (exist != null) return -1;

            var entity = _mapper.Map<User>(model);
            var result = await _userManager.CreateAsync(entity, password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(entity, isPersistent: false);
                return entity.Id;
            }
            return 0;
        }

        public async Task<int> Login(string userName, string password)
        {
           var result = await _signInManager.PasswordSignInAsync(userName, password, true, false);
            return result.Succeeded ? 0 : -1;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
