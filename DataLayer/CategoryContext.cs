using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class CategoryContext: IDb<Category, int>
    {
        private ExpenseTrackerDbContext dbContext;
        public CategoryContext(ExpenseTrackerDbContext appContext)
        {
            dbContext = appContext;
        }
        public void Create(Category item)
        {
            dbContext.Categories.Add(item);
            dbContext.SaveChanges();
        }
        public Category Read(int id, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Category> query = dbContext.Categories;
            if (useNavigationalProperties)
            {

            }
            if (isReadOnly)
            {
                query = query.AsNoTrackingWithIdentityResolution();
            }
            Category Category = query.FirstOrDefault(f => f.Id == id);
            if (Category == null)
            {
                throw new Exception("Category not found!");
            }
            return Category;
        }
        public List<Category> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Category> query = dbContext.Categories;
            if (useNavigationalProperties)
            {

            }
            if (isReadOnly)
            {
                query = query.AsNoTrackingWithIdentityResolution();
            }
            return query.ToList();
        }
        public void Update(Category Category, bool useNavigationalProperties = false)
        {
            Category CategoryFromContext = dbContext.Categories.Find(Category.Id);
            CategoryFromContext.Name = Category.Name;

            dbContext.SaveChanges();
        }
        public void Delete(int id)
        {
            Category category = dbContext.Categories.Find(id);
            if (category != null)
            {
                dbContext.Categories.Remove(category);
                dbContext.SaveChanges();
            }
            else
            {
                throw new Exception("Category not found!");
            }
        }
    }
}

