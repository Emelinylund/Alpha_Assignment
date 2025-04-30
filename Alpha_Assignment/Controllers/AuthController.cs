using Alpha_Assignment.Models;
using Business.Services;
using DataClass.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authentication;

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

                // Skapa principal med claim, Chat GPT helped me with this
                var principal = await _signInManager.CreateUserPrincipalAsync(user);
                var identity = (System.Security.Claims.ClaimsIdentity)principal.Identity!;
                identity.AddClaim(new System.Security.Claims.Claim("FullName", $"{user.FirstName} {user.LastName}"));

                await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);

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
                    var principal = await _signInManager.CreateUserPrincipalAsync(user);
                    var identity = (System.Security.Claims.ClaimsIdentity)principal.Identity!;

                    // Lägg till FullName-claim, Chat GPT helped me with this
                    if (!identity.HasClaim(c => c.Type == "FullName"))
                    {
                        identity.AddClaim(new System.Security.Claims.Claim("FullName", $"{user.FirstName} {user.LastName}"));
                    }

                    await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);
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
