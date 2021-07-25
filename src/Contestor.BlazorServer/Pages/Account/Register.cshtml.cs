using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contestor.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Contestor.BlazorServer.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        IUserService _userService;

        public RegisterModel(IUserService userService)
        {
            _userService = userService;
        }

        public string ReturnUrl { get; set; }
        [BindProperty]
        public RegisterViewModel Input { get; set; }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                await _userService.Register(Input);
                return Redirect("~/");
                
            }
            return Page();
        }
    }
}
