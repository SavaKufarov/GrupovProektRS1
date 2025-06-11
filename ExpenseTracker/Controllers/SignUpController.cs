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

        public SignUpController(UserContext userContext)
        {
            _userContext = userContext;
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
                return View("SignUp"); // Or redirect to login page
            }

            try
            {
                // Find user by email
               

                // Attempt to sign in
                var result = await _userContext.LogInUserAsync(email, password);

                if (result.Succeeded)
                {
                    // Redirect to home page or dashboard
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("password", "Invalid email or password.");
                    return View("SignUp"); // Or redirect to login page
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred during login.");
                // Log the exception
                return View("SignUp"); // Or redirect to login page
            }
        }
    }
}