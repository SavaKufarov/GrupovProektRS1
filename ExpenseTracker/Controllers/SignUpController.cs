using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DataLayer;
using BusinessLayer;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Controllers
{
    public class SignUpController : Controller
    {
        private readonly UserContext _userContext;
        private readonly SignInManager<User> _signInManager;

        public SignUpController(UserContext userContext, SignInManager<User> signInManager)
        {
            _userContext = userContext;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string email, string password, string confirmPassword)
        {
            // Validate inputs
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                ModelState.AddModelError("", "All fields are required.");
                return View("SignUp");
            }

            if (password != confirmPassword)
            {
                ModelState.AddModelError("confirmPassword", "Passwords do not match.");
                return View("SignUp");
            }

            try
            {
                // Check if user already exists
                var existingUser = await _userContext.FindUserByNEmailAsync(email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("email", "Email is already registered.");
                    return View("SignUp");
                }

                // Create new user
                var user = new User
                {
                    UserName = username,
                    Email = email,
                    EmailConfirmed = true // Set to true if you're not implementing email confirmation
                };

                await _userContext.CreateUserAsync(user, password);

                // Redirect to success page or login page
                return View("SignUp");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Your password is incorrect", ex.Message);
                return View("SignUp");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, bool rememberMe)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Email and password are required.");
                return View("SignUp");
            }

            try
            {
                var user = await _userContext.FindUserByNEmailAsync(email);
                if (user == null)
                {
                    ModelState.AddModelError("email", "User not found.");
                    return View("SignUp");
                }

                var signInResult = await _userContext.LogInUserAsync(email, password);

                if (signInResult != null)
                {
                    // Store user's name in a cookie
                    var cookieOptions = new CookieOptions
                    {
                        Expires = rememberMe ? DateTime.Now.AddDays(30) : DateTime.Now.AddMinutes(30),
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    };

                    Response.Cookies.Append("UserName", user.UserName, cookieOptions);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("password", "Invalid email or password.");
                    return View("SignUp");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred during login: {ex.Message}");
                return View("SignUp");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Sign out the user
            await _signInManager.SignOutAsync();

            // Clear the user name cookie
            Response.Cookies.Delete("UserName");

            // Redirect to home page
            return RedirectToAction("Index", "Home");
        }
    }
}