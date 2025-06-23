using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Note
    {
        [Key]
        public int Id { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Budget { get; set; }
        public string Description { get; set; } 

        
        public User User { get; set; }

        public Expense Expense { get; set; }

    }
}
