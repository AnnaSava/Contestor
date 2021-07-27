using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contestor.Data.Contract.Models;
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
        IContestService _contestService;

        public RegisterModel(IUserService userService, IContestService contestService)
        {
            _userService = userService;
            _contestService = contestService;
        }

        public string ReturnUrl { get; set; }

        public bool SendWork { get; set; }

        public ContestModel Contest { get; set; }

        [BindProperty]
        public RegisterViewModel Input { get; set; }

        public async void OnGetAsync(long? contestId, bool sendWork = false, string returnUrl = null)
        {
            SendWork = contestId.HasValue && sendWork;
            Contest = contestId.HasValue ? await _contestService.GetOne(contestId.Value) : null;
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                await _userService.Register(Input);
                if (SendWork && Contest != null)
                    return Redirect($"~/SendWork/{Contest.Id}");
                if (Contest != null)
                    return Redirect($"~/Contest/{Contest.Id}");
                return Redirect("~/");
            }
            return Page();
        }
    }
}
