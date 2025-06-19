using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Budget
    {
        public int Id { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }
        public DateTime Month { get; set; } // Set to first day of the month for consistency

        // Foreign keys
        public int UserId { get; set; }
        public User User { get; set; }

        public int? CategoryId { get; set; } // Optional: budget for a specific category
        public Category Category { get; set; }
    }
}
