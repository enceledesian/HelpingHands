using HelpingHands.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HelpingHands.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public AccountController(UserManager<IdentityUser> _userManager, SignInManager<IdentityUser> _signInManager, RoleManager<IdentityRole> _roleManager)
        {
            this.userManager = _userManager;
            this.signInManager = _signInManager;
            this.roleManager = _roleManager;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        /// <summary>
        /// User can register with basic details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(Register model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isRoleExists = await roleManager.RoleExistsAsync("User");
                    if (!isRoleExists)
                    {
                        // Create User Role
                        var userRole = new IdentityRole();
                        userRole.Name = "User";
                        await roleManager.CreateAsync(userRole);
                    }
                    var user = new IdentityUser { UserName = model.Email, Email = model.FullName, PhoneNumber = model.MobileNumber };
                    var userResult = await userManager.CreateAsync(user, model.Password);

                    if (userResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "User");
                        await signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }
                    foreach (var error in userResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

            }
            catch (Exception ex)
            {
                return View("Error");
            }
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// Get External Social Login Details
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            try
            {
                Login loginModel = new Login
                {
                    ReturnUrl = returnUrl,
                    SocialLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
                };
                return View(loginModel);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
        /// <summary>
        /// User can login with id and password
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Login loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userResult = await signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, false, false);
                    if (userResult.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("", "Invalid Login");
                }
            }
            catch (Exception ex)
            {
                return View("Error");
            }

            return View(loginModel);
        }
        /// <summary>
        /// Redirect to Google
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult GoogleLogin(string provider, string returnUrl)
        {
            try
            {
                var redirectUrl = Url.Action("GoogleCallback", "Account",
                                    new { ReturnUrl = returnUrl });

                var properties =
                    signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
                return new ChallengeResult(provider, properties);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
        /// <summary>
        /// External Google Login
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> GoogleCallback(string returnUrl = null)
        {
            try
            {
                returnUrl = returnUrl ?? Url.Content("/Home/Index");
                Login loginModel = new Login
                {
                    ReturnUrl = returnUrl,
                    SocialLogins =
                            (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
                };

                // Get the login information about the user from the google login provider
                var googleInfo = await signInManager.GetExternalLoginInfoAsync();
                if (googleInfo == null)
                {
                    ModelState
                        .AddModelError(string.Empty, "Error loading google login information.");

                    return View("Login", loginModel);
                }

                // If the user already has a login then sign-in the user with this google login provider
                var googleSignInResult = await signInManager.ExternalLoginSignInAsync(googleInfo.LoginProvider,
                    googleInfo.ProviderKey, isPersistent: false, bypassTwoFactor: true);

                if (googleSignInResult.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    var email = googleInfo.Principal.FindFirstValue(ClaimTypes.Email);

                    if (email != null)
                    {
                        // Create a new user without password if we do not have a user already
                        var user = await userManager.FindByEmailAsync(email);

                        if (user == null)
                        {
                            user = new IdentityUser
                            {
                                UserName = googleInfo.Principal.FindFirstValue(ClaimTypes.Email),
                                Email = googleInfo.Principal.FindFirstValue(ClaimTypes.Name)
                            };
                            // Create new user and Assign User Role
                            await userManager.CreateAsync(user);
                            await userManager.AddToRoleAsync(user, "User");
                        }

                        // Add a login insert a row for the user in AspNetUserLogins table
                        await userManager.AddLoginAsync(user, googleInfo);
                        await signInManager.SignInAsync(user, isPersistent: false);

                        return LocalRedirect(returnUrl);
                    }
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                //ViewBag.Error = ex.ToString();
                return View("Error");
            }
            
        }


    }
}
