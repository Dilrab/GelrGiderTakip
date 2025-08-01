namespace ApiGelirGider.DTOs.Expense
{
    public class ExpenseDto
    {
        public int ExpenseId { get; set; }                      // Gider kaydının ID’si
        public decimal ExpenseAmount { get; set; }              // Tutar
        public string CategoryName { get; set; }         // Kategori adı
        public DateTime ExpenseDate { get; set; }               // Tarih
    }
}
