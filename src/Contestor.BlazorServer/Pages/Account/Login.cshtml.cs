using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Contestor.Proto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
                    ModelState.AddModelError("", "������ �����: �������� ����� ��� ������");
                    return Page();
                }

                //var user = await _userService.Auth(Input);
                //if (user == null)
                //{
                //    ModelState.AddModelError("", "������ �����: �������� ����� ��� ������");
                //    return Page();
                //}

                //IEnumerable<Claim> claims = new List<Claim>
                //{
                //    new Claim(ClaimTypes.NameIdentifier, user.UserName)
                //};
                //var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                //var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties());

                return Redirect("~/");

                //if (result.Succeeded)
                //{
                //    // ���������, ����������� �� URL ����������
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
                //    ModelState.AddModelError("", "������������ ����� � (���) ������");
                //}
            }
            return Page();
        }
    }
}
