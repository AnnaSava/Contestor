using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contestor.Service.Contract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Contestor.BlazorServer.Pages.Account
{
    [IgnoreAntiforgeryToken]
    public class LogoutModel : PageModel
    {
        private readonly IUserService _userService;

        public LogoutModel(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // удаляем аутентификационные куки
            await _userService.Logout();
            return Redirect("~/");
        }
    }
}
