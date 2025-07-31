namespace ApiGelirGider.WebApi.DTOs.Income
{
    public class IncomeCreateDto
    {
        public decimal IncomeAmount { get; set; }               // Girilecek miktar
        public int CategoryId { get; set; }    // Kategori FK
        public int UserId { get; set; } // ← Bu mutlaka olmalı
        public DateTime IncomeDate { get; set; }                // Gelir tarihi
    }
}
