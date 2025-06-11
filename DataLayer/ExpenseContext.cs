using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class ExpenseContext : IDb<Expense, int>
    {
        private ExpenseTrackerDbContext dbContext;
        public ExpenseContext(ExpenseTrackerDbContext appContext)
        {
            dbContext = appContext;
        }
        public void Create(Expense item)
        {
            dbContext.Expenses.Add(item);
            dbContext.SaveChanges();
        }
        public Expense Read(int id, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Expense> query = dbContext.Expenses;
            if (useNavigationalProperties)
            {
                
            }
            if (isReadOnly)
            {
                query = query.AsNoTrackingWithIdentityResolution();
            }
            Expense expense = query.FirstOrDefault(f => f.Id == id);
            if (expense == null)
            {
                throw new Exception("District not found!");
            }
            return expense;
        }
        public List<Expense> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Expense> query = dbContext.Expenses;
            if (useNavigationalProperties)
            {
               
            }
            if (isReadOnly)
            {
                query = query.AsNoTrackingWithIdentityResolution();
            }
            return query.ToList();
        }
        public void Update(Expense expense, bool useNavigationalProperties = false)
        {
            Expense expenseFromContext = dbContext.Expenses.Find(expense.Id);
            expenseFromContext.Name = expense.Name;
            
            dbContext.SaveChanges();
        }
        public void Delete(int id)
        {
            Expense district = dbContext.Expenses.Find(id);
            if (district != null)
            {
                dbContext.Expenses.Remove(district);
                dbContext.SaveChanges();
            }
            else
            {
                throw new Exception("District not found!");
            }
        }
    }
}
