using GrupovProektRS1.Models;
using Microsoft.EntityFrameworkCore;

namespace GrupovProektRS1.Data
{
    public class ExpenseContext:DbContext
    {
        public ExpenseContext(DbContextOptions<ExpenseContext> options) : base(options) { }

        public DbSet<Expense> Expenses { get; set; }
    }
}
