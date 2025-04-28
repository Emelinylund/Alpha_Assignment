using Alpha_Assignment.Models;
using Business.Services;
using DataClass.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace Alpha_Assignment.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly UserManager<UserEntity> _userManager;

        public AuthController(IAuthService authService, SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager)
        {
            _authService = authService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // Register GET
        public IActionResult Register()
        {
            var formData = new RegisterFormModel();
            return View(formData);
        }

        // Register POST
        [HttpPost]
        public async Task<IActionResult> Register(RegisterFormModel formData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    errors = ModelState.Where(m => m.Value?.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                        )
                });
            }

            var user = new UserEntity
            {
                UserName = formData.Email,
                Email = formData.Email,
                FirstName = formData.FirstName,
                LastName = formData.LastName
            };

            var result = await _userManager.CreateAsync(user, formData.Password);

            if (result.Succeeded)
            {
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("FullName", $"{user.FirstName} {user.LastName}"));
                // Log in the user after registration
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect("/projects");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(new
            {
                errors = ModelState
                    .Where(m => m.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    )
            });
        }

        // Login GET
        public IActionResult Login()
        {
            return View();
        }

        // Login POST
        [HttpPost]
        public async Task<IActionResult> Login(LoginFormModel formData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    errors = ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .ToDictionary(
                            x => x.Key,
                            x => x.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                        )
                });
            }

            var result = await _signInManager.PasswordSignInAsync(
                formData.Email,
                formData.Password,
                formData.RememberMe,
                lockoutOnFailure: false
            );

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(formData.Email);

                if (user != null)
                {
                    var existingClaims = await _userManager.GetClaimsAsync(user);
                    var fullNameClaim = existingClaims.FirstOrDefault(c => c.Type == "FullName");

                    if (fullNameClaim == null)
                    {
                        await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("FullName", $"{user.FirstName} {user.LastName}"));
                    }
                }

                return LocalRedirect("/projects");
            }


            return BadRequest(new
            {
                errors = new Dictionary<string, string[]>
                {
                    { "Password", new[] { "Invalid email or password." } }
                }
            });
        }

        // SignOut
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login","Auth");
        }
    }
}
