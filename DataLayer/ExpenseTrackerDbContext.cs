using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BusinessLayer;

namespace DataLayer
{
    public class ExpenseTrackerDbContext : IdentityDbContext<User>
    {
        public DbSet<Expense> Expenses { get; set; }
        public ExpenseTrackerDbContext() : base()
        { }
        public ExpenseTrackerDbContext(DbContextOptions<ExpenseTrackerDbContext> options)
     : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder != null)
            {
                optionsBuilder.UseSqlite("Data Source=expense_tracker.db");
            }
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
