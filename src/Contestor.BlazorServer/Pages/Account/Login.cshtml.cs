using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contestor.Proto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Contestor.BlazorServer.Pages.Account
{
    public class LoginModel : PageModel
    {
        IUserViewService _userService;

        public LoginModel(IUserViewService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public LoginViewModel Input { get; set; }

        public void OnGet(string returnUrl = null)
        {
            Input = new LoginViewModel() { ReturnUrl = returnUrl };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Login(Input);
                if (result == -1)
                {
                    ModelState.AddModelError("", "Ошибка входа: неверный логин или пароль");
                    return Page();
                }
                return Redirect("~/");
                //if (result.Succeeded)
                //{
                //    // проверяем, принадлежит ли URL приложению
                //    if (!string.IsNullOrEmpty(Input.ReturnUrl) && Url.IsLocalUrl(Input.ReturnUrl))
                //    {
                //        return Redirect(Input.ReturnUrl);
                //    }
                //    else
                //    {
                //        return RedirectToPage("/Index");
                //    }
                //}
                //else
                //{
                //    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                //}
            }
            return Page();
        }
    }
}
