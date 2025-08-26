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

    }
}
