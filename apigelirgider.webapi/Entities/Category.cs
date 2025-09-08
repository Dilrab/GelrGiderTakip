using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IncomeExpenseTracker.Entities;


namespace IncomeExpenseTracker.Entities
{
    // Gelir ve giderlerin gruplandırıldığı kategori yapısı
    public class Category
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false ,ErrorMessage ="Adı Boş geçilemez")]
        public string CategoryName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "tipi Boş geçilemez")]
        public int Type { get; set; }
        public int UserId { get; set; }

        // İlişkili gelir ve gider kayıtları
        // public ICollection<Income> Incomes { get; set; } = new List<Income>();
        // public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
