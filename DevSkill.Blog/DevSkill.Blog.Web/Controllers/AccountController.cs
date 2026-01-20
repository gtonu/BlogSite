using DevSkill.Blog.Domain.Utilities;
using DevSkill.Blog.Infrastructure.Identity;
using DevSkill.Blog.Web.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace DevSkill.Blog.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<BlogSiteUser> _signInManager;
        private readonly UserManager<BlogSiteUser> _userManager;
        private readonly IUserStore<BlogSiteUser> _userStore;
        private readonly IUserEmailStore<BlogSiteUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailUtility _emailUtility;
        private IMapper _mapper;
        public AccountController(
            UserManager<BlogSiteUser> userManager,
            IUserStore<BlogSiteUser> userStore,
            SignInManager<BlogSiteUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailUtility emailUtility,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailUtility = emailUtility;
            _mapper = mapper;
        }
        public async Task<IActionResult> RegisterAsync(string returnUrl = null)
        {
            var model = new RegisterModel();

            model.ReturnUrl = returnUrl;
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync(RegisterModel model)
        {
            model.ReturnUrl ??= Url.Content("~/");
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                //var user = CreateUser();
                var user = _mapper.Map<RegisterModel, BlogSiteUser>(model);
                if(model.PhoneNumber != null)
                {
                    user.PhoneNumberConfirmed = true;
                }

                await _userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, model.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = model.ReturnUrl },
                        protocol: Request.Scheme);

                    await _emailUtility.SendEmailAsync(model.FirstName,model.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = model.Email, returnUrl = model.ReturnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(model.ReturnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        public async Task<IActionResult> LoginAsync(string returnUrl = null)
        {
            var model = new LoginModel();
            if (!string.IsNullOrEmpty(model.ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, model.ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            model.ReturnUrl = returnUrl;
            return View(model);
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(LoginModel model)
        {
            model.ReturnUrl ??= Url.Content("~/");

            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(model.ReturnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        public IActionResult ExternalLogin(ExternalLoginModel model)
        {
            if (!string.IsNullOrEmpty(model.ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, model.ErrorMessage);
            }

            return LocalRedirect(model.ReturnUrl);
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginAsync(string provider, ExternalLoginModel model)
        {
            model.ReturnUrl ??= Url.Content("~/");
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", values: new { model.ReturnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }
        public async Task<IActionResult> ExternalLoginCallbackAsync(ExternalLoginModel model,string returnUrl = null, string remoteError = null)
        {
            //var model = new ExternalLoginModel();
            //if we create a new instance as above we will lose the returnUrl..so we have to catch the model in parameter..

            model.ReturnUrl ??= Url.Content("~/");
            if (remoteError != null)
            {
                model.ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction("Login", "Account", new { ReturnUrl = model.ReturnUrl });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                model.ErrorMessage = "Error loading external login information.";
                return RedirectToAction("Login", "Account", new { ReturnUrl = model.ReturnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(model.ReturnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                model.ReturnUrl = returnUrl;
                model.ProviderDisplayName = info.ProviderDisplayName;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    model.Email = info.Principal.FindFirstValue(ClaimTypes.Email);
                }
                return View("ExternalLogin", model);
            }
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmationAsync(ExternalLoginModel model)
        {
            model.ReturnUrl ??= Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                model.ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToAction("Login", "Account", new { ReturnUrl = model.ReturnUrl });
            }

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, model.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Action(
                            "ConfirmEmail",
                            "Account",
                            values: new { area = "Identity", userId = userId, code = code },
                            protocol: Request.Scheme);

                        await _emailUtility.SendEmailAsync(model.Email,model.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        // If account confirmation is required, we need to show the link if we don't have a real email sender
                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("./RegisterConfirmation", new { Email = model.Email });
                        }

                        await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                        return LocalRedirect(model.ReturnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            model.ProviderDisplayName = info.ProviderDisplayName;
            return View("ExternalLogin",model);
        }

        public async Task<IActionResult> ConfirmEmailAsync(string userId, string code)
        {
            var model = new ConfirmEmailModel();
            if (userId == null || code == null)
            {
                return RedirectToAction("Register");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            model.StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            return View();
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        public async Task<IActionResult> ForgotPasswordAsync()
        {
            var model = new ForgotPasswordModel();
            return View(model);
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Action(
                    "ResetPassword",
                    "Account",
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                await _emailUtility.SendEmailAsync(model.Email,
                    model.Email,
                    "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return RedirectToAction("ForgotPasswordConfirmation");
            }

            return View(model);
        }
        public async Task<IActionResult> ResetPasswordAsync(string code = null)
        {
            var model = new ResetPasswordModel();
            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                model.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                return View(model);
            }
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }
        
        public async Task<IActionResult> LogoutAsync(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                //no specific view() is required for Logout action because user will be redirected to home page
                //after logging out..RedirectToAction() redirects user to home page..
                return RedirectToAction();
            }
        }

        private BlogSiteUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<BlogSiteUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(BlogSiteUser)}'. " +
                    $"Ensure that '{nameof(BlogSiteUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<BlogSiteUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<BlogSiteUser>)_userStore;
        }
        
    }
}
