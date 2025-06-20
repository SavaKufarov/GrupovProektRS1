using BusinessLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class UserContext
    {
        UserManager<User> _userManager;
        ExpenseTrackerDbContext _context;

        public UserContext(ExpenseTrackerDbContext context, UserManager<User> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        #region CRUD

        public async Task CreateUserAsync(User user, string password)
        {
            try
            {
                IdentityResult result = await _userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    throw new ArgumentException(result.Errors.First().Description);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User> LogInUserAsync(string email, string password)
        {
            try
            {
                User user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return null;
                }

                // Проверка на паролата с правилния метод
                var passwordValid = await _userManager.CheckPasswordAsync(user, password);
                return passwordValid ? user : null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error during Login", ex);
            }
        }


        public async Task<User> ReadUserAsync(string key)
        {
            try
            {
                return await _userManager.FindByIdAsync(key);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<User>> ReadAllUsersAsync()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

     
        public async Task DeleteUserByNameAsync(string name)
        {
            try
            {
                User user = await FindUserByNEmailAsync(name);

                if (user == null)
                {
                    throw new InvalidOperationException("User not found for deletion!");
                }

                await _userManager.DeleteAsync(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<User> FindUserByNEmailAsync(string name)
        {
            try
            {
                // Identity return Null if there is no user!
                return await _userManager.FindByEmailAsync(name);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

    #endregion
}