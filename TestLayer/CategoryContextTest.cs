using DataLayer;
using BusinessLayer;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;

namespace TestLayer
{
    [TestFixture]
    public class CategoryContextTest
    {
        private CategoryContext categoryContext;

        [OneTimeSetUp]
        public void Setup()
        {
            categoryContext = new CategoryContext(TestManager.dbContext);
            TestManager.dbContext.Categories.RemoveRange(TestManager.dbContext.Categories);
            TestManager.dbContext.SaveChanges();
        }

        [Test]
        public void CreateCategory()
        {
            Category category = new Category("Food");
            int categoriesBefore = TestManager.dbContext.Categories.Count();
            categoryContext.Create(category);
            int categoriesAfter = TestManager.dbContext.Categories.Count();
            Category lastCategory = TestManager.dbContext.Categories.Last();

            Assert.That(categoriesBefore + 1 == categoriesAfter && lastCategory.Name == category.Name,
                "Create() does not add a category or names don't match!");
        }

        [Test]
        public void DeleteCategory()
        {
            Category newCategory = new Category("Transportation");
            categoryContext.Create(newCategory);

            List<Category> categories = categoryContext.ReadAll();
            int categoriesBefore = categories.Count;
            Category categoryToDelete = categories.Last();

            categoryContext.Delete(categoryToDelete.Id);

            int categoriesAfter = categoryContext.ReadAll().Count;
            Assert.That(categoriesBefore == categoriesAfter + 1,
                "Delete() does not remove a category!");
        }

        [Test]
        public void ReadCategory()
        {
            Category newCategory = new Category("Utilities");
            categoryContext.Create(newCategory);

            Category retrievedCategory = categoryContext.Read(newCategory.Id);

            Assert.That(retrievedCategory.Name == newCategory.Name,
                "Read() does not get category by id correctly!");
        }

        [Test]
        public void ReadAllCategories()
        {
            TestManager.dbContext.Categories.RemoveRange(TestManager.dbContext.Categories);
            TestManager.dbContext.SaveChanges();

            categoryContext.Create(new Category("Category 1"));
            categoryContext.Create(new Category("Category 2"));

            int categoriesCount = categoryContext.ReadAll().Count;

            Assert.That(categoriesCount == 2,
                "ReadAll() does not return all categories!");
        }

        [Test]
        public void UpdateCategory()
        {
            Category newCategory = new Category("Entertainment");
            categoryContext.Create(newCategory);

            Category lastCategory = categoryContext.ReadAll().Last();
            lastCategory.Name = "Updated Category Name";

            categoryContext.Update(lastCategory);

            Assert.That(categoryContext.Read(lastCategory.Id).Name == "Updated Category Name",
                "Update() does not modify the category correctly!");
        }

        [Test]
        public void ReadNonExistentCategoryThrowsException()
        {
            Assert.Throws<Exception>(() => categoryContext.Read(-1),
                "Read() should throw exception for non-existent category ID");
        }

        [Test]
        public void DeleteNonExistentCategoryThrowsException()
        {
            Assert.Throws<Exception>(() => categoryContext.Delete(-1),
                "Delete() should throw exception for non-existent category ID");
        }
    }
}