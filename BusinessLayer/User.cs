using Microsoft.AspNetCore.Identity;

namespace BusinessLayer
{
    public class User:IdentityUser
    {
        public List<Expense> Expenses { get; set; }

        public User()
        {
            Expenses = new List<Expense>();
        }

        public User(string userName, string email, string password, List<Expense> expenses)
        {
            Expenses = expenses;
            Email = email;
            UserName = userName;
            PasswordHash = password;
        }
    }
}
