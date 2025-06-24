using BusinessLayer;

namespace ExpenseTracker.Models
{
    public class HomeViewModel
    {
        public List<Expense> MonthExpenses { get; set; }
        public List<Expense> WeekExpenses { get; set; }
        public decimal MonthTotal { get; set; }
        public decimal WeekTotal { get; set; }
        public string CurrentMonth { get; set; }
        public string CurrentWeek { get; set; }
    }
}
