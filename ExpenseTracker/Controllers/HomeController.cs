using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DataLayer;
using System.Diagnostics;
using ExpenseTracker.Models;
using BusinessLayer;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ExpenseContext _expenseContext;
        private readonly ExpenseTrackerDbContext _dbContext;
        private readonly NoteContext _noteContext;

        public HomeController(ILogger<HomeController> logger,
                            ExpenseContext expenseContext,
                            ExpenseTrackerDbContext dbContext,
                            NoteContext noteContext)
        {
            _logger = logger;
            _expenseContext = expenseContext;
            _dbContext = dbContext;
            _noteContext = noteContext;
        }

        public IActionResult Index()
        {
            var now = DateTime.Now;

       
            ViewBag.CurrentMonth = now.ToString("MMMM yyyy");

            var startOfWeek = now.AddDays(-(int)now.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(6);
            ViewBag.CurrentWeek = $"{startOfWeek:MMM dd} - {endOfWeek:MMM dd, yyyy}";

            var monthExpenses = _dbContext.Expenses
                .Where(e => e.Date.Month == now.Month && e.Date.Year == now.Year)
                .Join(_dbContext.Categories,
                    expense => expense.CategoryId,
                    category => category.Id,
                    (expense, category) => new {
                        Expense = expense,
                        CategoryName = category.Name
                    })
                .OrderByDescending(e => e.Expense.Date)
                .ToList();
            ViewBag.MonthTotal = monthExpenses.Sum(e => e.Expense.Amount);
            ViewBag.MonthExpenses = monthExpenses;

           
            var weekExpenses = _dbContext.Expenses
                .Where(e => e.Date >= startOfWeek && e.Date <= endOfWeek)
                .Join(_dbContext.Categories,
                    expense => expense.CategoryId,
                    category => category.Id,
                    (expense, category) => new {
                        Expense = expense,
                        CategoryName = category.Name
                    })
                .OrderByDescending(e => e.Expense.Date)
                .ToList();
            ViewBag.WeekTotal = weekExpenses.Sum(e => e.Expense.Amount);
            ViewBag.WeekExpenses = weekExpenses;

            return View();
        }

        [HttpGet]
        public IActionResult GetNote(int expenseId)
        {

            var note = _dbContext.Notes.FirstOrDefault(n => n.Expense != null && n.Expense.Id == expenseId);
            if (note != null)
            {
                return Json(new
                {
                    budget = note.Budget,
                    description = note.Description
                });
            }
            return Json(null);
        }

        [HttpPost]
        public IActionResult SaveNote([FromBody] NoteModel model)
        {
            try
            {
                var existingNote = _dbContext.Notes.FirstOrDefault(n => n.Expense != null && n.Expense.Id == model.ExpenseId);

                if (existingNote != null)
                {
                    
                    existingNote.Budget = model.Budget;
                    existingNote.Description = model.Description;
                    _noteContext.Update(existingNote);
                }
                else
                {
                    
                    var expense = _expenseContext.Read(model.ExpenseId);
                    var newNote = new Note
                    {
                        Budget = model.Budget,
                        Description = model.Description,
                        Expense = expense
                    };
                    _noteContext.Create(newNote);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var expense = _dbContext.Expenses.FirstOrDefault(e => e.Id == id);

            if (expense == null)
            {
                return NotFound();
            }

            
            ViewBag.Categories = _dbContext.Categories.ToList();
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
                    _dbContext.Update(expense);
                    _dbContext.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
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
            }

            
            ViewBag.Categories = _dbContext.Categories.ToList();
            return View(expense);
        }

        private bool ExpenseExists(int id)
        {
            return _dbContext.Expenses.Any(e => e.Id == id);
        }

        [HttpPost]

        public IActionResult Delete(int id)
        {
            try
            {
                var expense = _dbContext.Expenses.FirstOrDefault(e => e.Id == id);
                if (expense == null)
                {
                    return NotFound();
                }

                _dbContext.Expenses.Remove(expense);
                _dbContext.SaveChanges();

                return Ok(); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error deleting expense"); 
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}