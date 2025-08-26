using System;

namespace IncomeExpenseTracker.Entities
{
    // Kullanıcının elde ettiği gelirleri tutar
    public class Income
    {
        public int IncomeId { get; set; } // PK - Her gelirin eşsiz ID'si
        public decimal IncomeAmount { get; set; } // Gelir tutarı
        public DateTime IncomeDate { get; set; } // Gelirin tarihi
        public string? IncomeSource { get; set; } 

        // Kategori ile ilişki
        public int CategoryId { get; set; } // FK - Hangi kategoriye ait olduğu
        public Category? Category { get; set; } // Navigation - Kategori nesnesi

        // Kullanıcı ile ilişki
       /// <summary>
        public int UserId { get; set; } // FK - Hangi kullanıcıya ait
       /// </summary>
        public User? User { get; set; } // Navigation - Kullanıcı nesnesi
    }
}
