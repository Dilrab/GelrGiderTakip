
using System;

namespace IncomeExpenseTracker.Entities
{
    // Kullanıcının yaptığı harcamaları tutar
    public class Expense
    {
        public int ExpenseId { get; set; } // PK
        public decimal ExpenseAmount { get; set; } // Gider tutarı
        public DateTime ExpenseDate { get; set; } // Gider tarihi
      
        // Kategori ile ilişki
       public int CategoryId { get; set; } // FK
       public Category? Category { get; set; } // Navigation

        // Kullanıcı ile ilişki
        public int UserId { get; set; } // FK
        public User? User { get; set; } // Navigation
    }
}
