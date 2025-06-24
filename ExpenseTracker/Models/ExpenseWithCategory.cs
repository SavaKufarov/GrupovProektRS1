using BusinessLayer;

namespace ExpenseTracker.Models
{
    public class ExpenseWithCategory
    {
        public Expense Expense { get; set; }
        public string CategoryName { get; set; }
        public string CssClass => CategoryName.ToLower().Replace(" ", "-");
    }
}
