using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        // Foreign key
        public int UserId { get; set; }
        public User User { get; set; }

        // Navigation property
        public ICollection<Expense> Expenses { get; set; }
    }
}
