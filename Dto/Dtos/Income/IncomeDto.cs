
namespace ApiGelirGider.DTOs.Income
{
    public class IncomeDto
    {
        public int IncomeId { get; set; }                  // Gelir kaydının ID’si
        public decimal IncomeAmount { get; set; }          // Gelir miktarı
        public string?  CategoryName { get; set; }     // Kategori adı
        public DateTime IncomeDate { get; set; }           // Tarih
    }
}
