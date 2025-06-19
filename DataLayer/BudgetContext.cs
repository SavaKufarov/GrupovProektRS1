using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class BudgetContext: IDb<Budget, int>
    {
        private ExpenseTrackerDbContext dbContext;
        public BudgetContext(ExpenseTrackerDbContext appContext)
        {
            dbContext = appContext;
        }
        public void Create(Budget item)
        {
            dbContext.Budgets.Add(item);
            dbContext.SaveChanges();
        }
        public Budget Read(int id, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Budget> query = dbContext.Budgets;
            if (useNavigationalProperties)
            {

            }
            if (isReadOnly)
            {
                query = query.AsNoTrackingWithIdentityResolution();
            }
            Budget Budget = query.FirstOrDefault(f => f.Id == id);
            if (Budget == null)
            {
                throw new Exception("District not found!");
            }
            return Budget;
        }
        public List<Budget> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Budget> query = dbContext.Budgets;
            if (useNavigationalProperties)
            {

            }
            if (isReadOnly)
            {
                query = query.AsNoTrackingWithIdentityResolution();
            }
            return query.ToList();
        }
        public void Update(Budget Budget, bool useNavigationalProperties = false)
        {
            Budget BudgetFromContext = dbContext.Budgets.Find(Budget.Id);
            BudgetFromContext.Name = Budget.Name;

            dbContext.SaveChanges();
        }
        public void Delete(int id)
        {
            Budget district = dbContext.Budgets.Find(id);
            if (district != null)
            {
                dbContext.Budgets.Remove(district);
                dbContext.SaveChanges();
            }
            else
            {
                throw new Exception("District not found!");
            }
        }
    }
}

