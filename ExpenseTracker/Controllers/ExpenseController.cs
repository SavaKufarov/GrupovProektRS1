using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExpenseTracker.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly ExpenseContext _expenseContext;
        private readonly CategoryContext _categoryContext;

        public ExpenseController(ExpenseContext expenseContext, CategoryContext categoryContext)
        {
            _expenseContext = expenseContext;
            _categoryContext = categoryContext;
        }


        public IActionResult Expense()
        {
            var categories = _categoryContext.ReadAll();
            List<string> categoryNames = categories.Select(c => c.Name).ToList();
            ViewBag.CategoriesJson = JsonSerializer.Serialize(categoryNames);
            ViewBag.Categories = new SelectList(categories, "Id", "Name"); 
            return View();
        }



        [HttpPost]
       
        public IActionResult Expense(Expense expense) 
        {
            if (ModelState.IsValid)
            {
                try
                {
                  

                    if (expense.Date == default)
                    {
                        expense.Date = DateTime.Now;
                    }

                    _expenseContext.Create(expense);

                    TempData["SuccessMessage"] = "Expense added successfully!";
                    return RedirectToAction("Index","Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving the expense: " + ex.Message);
                }
            }

           
            return View();
        }


        public IActionResult Edit(int id)
        {
            var expense = _expenseContext.Read(id, useNavigationalProperties: true);
            if (expense == null)
            {
                return NotFound();
            }
            LoadCategories();
            return View(expense);
        }

        
        [HttpPost]
        
        public IActionResult Edit(int id, [Bind("Id,Name,Amount,Date,CategoryId")] Expense expense)
        {
            if (id != expense.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _expenseContext.Update(expense);
                }
                catch (Exception)
                {
                    if (!ExpenseExists(expense.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            LoadCategories();
            return View(expense);
        }

        
        public IActionResult Delete(int id)
        {
            var expense = _expenseContext.Read(id, useNavigationalProperties: true);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        
        [HttpPost, ActionName("Delete")]
       
        public IActionResult DeleteConfirmed(int id)
        {
            _expenseContext.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private bool ExpenseExists(int id)
        {
            return _expenseContext.ReadAll().Any(e => e.Id == id);
        }

        private void LoadCategories()
        {
            var categories = _categoryContext.ReadAll();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
        }
    }
}