namespace ApiGelirGider.DTOs.Expense
{
    public class ExpenseSummaryDto
    {
        public string ExpenseMonth { get; set; }                 // Örn: "2025-07"
        public decimal TotalExpense { get; set; }         // Aylık toplam gider
    }
}
