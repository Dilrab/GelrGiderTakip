using ApiGelirGider.DTOs.Expense;
using ApiGelirGider.DTOs.Income;
using System.Collections.Generic;

namespace ApiGelirGider.WebUI.Models
{
    public class DashboardViewModel
    {
        public List<ExpenseDto> LastExpenses { get; set; }
        public List<IncomeDto> LastIncomes { get; set; }
    }
}
