using System.Collections.Generic;
using IncomeExpenseTracker.Entities;


namespace IncomeExpenseTracker.Entities
{
    // Gelir ve giderlerin gruplandırıldığı kategori yapısı
    public class Category
    {
        public int CategoryId { get; set; } // PK
        public string? CategoryName { get; set; } // Kategori adı (örn: Market, Maaş)

        // İlişkili gelir ve gider kayıtları
       // public ICollection<Income> Incomes { get; set; } = new List<Income>();
       // public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
