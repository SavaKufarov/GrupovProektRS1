using DataLayer;
using BusinessLayer;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using System;

namespace TestLayer
{
    [TestFixture]
    public class ExpenseContextTest
    {
        private ExpenseContext expenseContext;
        private Category testCategory;

        [OneTimeSetUp]
        public void Setup()
        {
            
            expenseContext = new ExpenseContext(TestManager.dbContext);

            
            var categoryContext = new CategoryContext(TestManager.dbContext);
            testCategory = new Category("Test Category");
            categoryContext.Create(testCategory);

            
            TestManager.dbContext.Expenses.RemoveRange(TestManager.dbContext.Expenses);
            TestManager.dbContext.SaveChanges();
        }

        [Test]
        public void CreateExpense()
        {
            Expense expense = new Expense()
            {
                Name = "Groceries",
                Amount = 50.00m,
                CategoryId = testCategory.Id
            };

            int expensesBefore = TestManager.dbContext.Expenses.Count();
            expenseContext.Create(expense);
            int expensesAfter = TestManager.dbContext.Expenses.Count();
            Expense lastExpense = TestManager.dbContext.Expenses.Last();

            Assert.That(expensesBefore + 1 == expensesAfter &&
                       lastExpense.Name == expense.Name &&
                       lastExpense.Amount == expense.Amount &&
                       lastExpense.CategoryId == expense.CategoryId,
                "Create() does not add an expense or properties don't match!");
        }

        [Test]
        public void DeleteExpense()
        {
            Expense newExpense = new Expense()
            {
                Name = "Dinner",
                Amount = 30.00m,
                CategoryId = testCategory.Id
            };
            expenseContext.Create(newExpense);

            List<Expense> expenses = expenseContext.ReadAll();
            int expensesBefore = expenses.Count;
            Expense expenseToDelete = expenses.Last();

            expenseContext.Delete(expenseToDelete.Id);

            int expensesAfter = expenseContext.ReadAll().Count;
            Assert.That(expensesBefore == expensesAfter + 1,
                "Delete() does not remove an expense!");
        }

        [Test]
        public void ReadExpense()
        {
            Expense newExpense = new Expense()
            {
                Name = "Movie Tickets",
                Amount = 25.00m,
                CategoryId = testCategory.Id
            };
            expenseContext.Create(newExpense);

            Expense retrievedExpense = expenseContext.Read(newExpense.Id);

            Assert.That(retrievedExpense.Name == newExpense.Name &&
                       retrievedExpense.Amount == newExpense.Amount &&
                       retrievedExpense.CategoryId == newExpense.CategoryId,
                "Read() does not get expense by id correctly!");
        }

        [Test]
        public void ReadAllExpenses()
        {
           
            TestManager.dbContext.Expenses.RemoveRange(TestManager.dbContext.Expenses);
            TestManager.dbContext.SaveChanges();

            
            expenseContext.Create(new Expense() { Name = "Expense 1", Amount = 10.00m, CategoryId = testCategory.Id });
            expenseContext.Create(new Expense() { Name = "Expense 2", Amount = 20.00m, CategoryId = testCategory.Id });

            int expensesCount = expenseContext.ReadAll().Count;

            Assert.That(expensesCount == 2,
                "ReadAll() does not return all expenses!");
        }

        [Test]
        public void UpdateExpense()
        {
            Expense newExpense = new Expense()
            {
                Name = "Original Name",
                Amount = 15.00m,
                CategoryId = testCategory.Id
            };
            expenseContext.Create(newExpense);

            Expense lastExpense = expenseContext.ReadAll().Last();
            lastExpense.Name = "Updated Name";
            lastExpense.Amount = 20.00m;

            expenseContext.Update(lastExpense);

            Expense updatedExpense = expenseContext.Read(lastExpense.Id);
            Assert.That(updatedExpense.Name == "Updated Name" &&
                       updatedExpense.Amount == 20.00m,
                "Update() does not modify the expense correctly!");
        }

        [Test]
        public void ReadNonExistentExpenseThrowsException()
        {
            Assert.Throws<Exception>(() => expenseContext.Read(-1),
                "Read() should throw exception for non-existent expense ID");
        }

        [Test]
        public void DeleteNonExistentExpenseThrowsException()
        {
            Assert.Throws<Exception>(() => expenseContext.Delete(-1),
                "Delete() should throw exception for non-existent expense ID");
        }

        [Test]
        public void CreateExpenseWithInvalidAmountThrowsException()
        {
            Expense invalidExpense = new Expense()
            {
                Name = "Invalid",
                Amount = 0.00m,
                CategoryId = testCategory.Id
            };

            Assert.Throws<Exception>(() => expenseContext.Create(invalidExpense),
                "Should not allow creating expense with amount <= 0");
        }
    }
}