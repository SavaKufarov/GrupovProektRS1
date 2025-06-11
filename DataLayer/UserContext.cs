using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer
{
    public class UserContext : DbContext, IDb<User, int>
    {
        public DbSet<User> Users { get; set; }
        private ExpenseTrackerDbContext dbContext;


        // **Create**
        public void Create(User user)
        {
            Users.Add(user);
            SaveChanges();
        }

        // **Read (Single User by Key)**
        public User Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<User> query = dbContext.Users;
            if (useNavigationalProperties)
            {

            }
            if (isReadOnly)
            {
                query = query.AsNoTrackingWithIdentityResolution();
            }
            User user = query.FirstOrDefault(f => f.Id == key);
            if (user == null)
            {
                throw new Exception("District not found!");
            }
            return user; ;
        }

        // **ReadAll**
        public List<User> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            var query = Users.AsQueryable();

            if (useNavigationalProperties)
            {
                // Include related entities if needed
            }

            if (isReadOnly)
            {
                query = query.AsNoTracking();
            }

            return query.ToList();
        }

        // **Update**
        public void Update(User user, bool useNavigationalProperties = false)
        {
            var existingUser = Users.Find(user.Id);
            if (existingUser != null)
            {
                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;
                SaveChanges();
            }
        }

        // **Delete**
        public void Delete(int key)
        {
            var user = Users.Find(key);
            if (user != null)
            {
                Users.Remove(user);
                SaveChanges();
            }
        }
    }
}