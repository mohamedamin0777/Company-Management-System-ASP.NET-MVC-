using Demo.DAL.Entities;
using Demo.PL.Helper;
using Demo.PL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #region SignUp
            public IActionResult SignUp()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> SignUp(RegisterViewModel registerViewModel)
            {
                if(ModelState.IsValid)
                {
                    var user = new ApplicationUser
                    {
                        Email = registerViewModel.Email,
                        UserName = registerViewModel.Email.Split('@')[0],
                        IsAgree = registerViewModel.IsAgree
                    };

                    var result = await _userManager.CreateAsync(user, registerViewModel.Password);

                    if (result.Succeeded) 
                        return RedirectToAction("Login");
                    else
                    {
                        foreach(var error in result.Errors)
                            ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                return View(registerViewModel);
            }
        #endregion

        #region Login
        public IActionResult Login(string? ReturnUrl)
        {
            return View();
        }

        [HttpPost] 
        public async Task<IActionResult> Login(SignInViewModel signInViewModel)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(signInViewModel.Email);

                if (user is null)
                    ModelState.AddModelError("", "Email Does Not Exist");

                var isCorrectPassword = await _userManager.CheckPasswordAsync(user, signInViewModel.Password);

                if (isCorrectPassword)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, signInViewModel.Password, signInViewModel.RememberMe, false);

                    if (result.Succeeded)
                        return RedirectToAction("Index", "Home");
                }

            }
            return View(signInViewModel);
        }
        #endregion

        #region SignOut
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        #endregion

        #region Forget Password
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if(user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);   
                    var resetPasswordLink = Url.Action("ResetPassword", "Account", new { Email = model.Email, Token = token }, Request.Scheme);

                    var email = new Email
                    {
                        Title = "Reset Password",
                        Body = resetPasswordLink,
                        To = model.Email
                    };

                    EmailSettings.SendEmail(email);

                    return RedirectToAction("CompleteForgetPassword");
                }

                ModelState.AddModelError("", "Invalid Email");
            }

            return View(model);
        }

        public IActionResult CompleteForgetPassword()
        {
            return View();
        }


        #endregion

        #region Reset Password
        public IActionResult ResetPassword(string email, string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

                    if (result.Succeeded)
                        return RedirectToAction(nameof(Login));

                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        } 
        #endregion

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
