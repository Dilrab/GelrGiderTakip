namespace ApiGelirGider.DTOs.Expense
{
    public class ExpenseCreateDto
    {
        public decimal ExpenseAmount { get; set; }               // Kullanıcının harcama miktarı
        public int CategoryId { get; set; }       // Kategori FK
        public int UserId { get; set; } // ← Bu mutlaka olmalı

        public DateTime ExpenseDate { get; set; }                // Harcama tarihi
    }
}
