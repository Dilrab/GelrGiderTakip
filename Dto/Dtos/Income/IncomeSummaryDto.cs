namespace ApiGelirGider.WebApi.DTOs.Income
{
    public class IncomeSummaryDto
    {
        public string IncomeMonth { get; set; }                 // Örn: "2025-07"
        public decimal TotalIncome { get; set; }          // Aylık toplam gelir
    }
}
