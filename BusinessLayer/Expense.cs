using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Expense
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        
        public int CategoryId { get; set; }

        
        public Category Category { get; set; }
    }
}
