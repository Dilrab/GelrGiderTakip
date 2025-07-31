using System.Collections.Generic;

namespace IncomeExpenseTracker.Entities
{
    // Sisteme giriş yapan kullanıcı bilgileri
    public class User
    {
        public int Id { get; set; } // PK
        public string? UserName { get; set; } // Ad Soyad
        public string? UserEmail { get; set; } // Kullanıcı email
        public string? Password { get; set; } // Şifrelenmiş parola

        // Gelir ve giderlerle bağlantı
      //   public ICollection<Income> Incomes { get; set; } = new List<Income>();
       // public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
