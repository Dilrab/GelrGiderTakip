using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGelirGider.DTOs.Expense;

namespace ApiGelirGider.Services.Interfaces
{
    public interface EExpenseService
    {
        Task<List<ExpenseDto>> GetAllAsync();
        Task<ExpenseDto> GetByIdAsync(int id);
        Task AddAsync(ExpenseDto dto);
        Task DeleteAsync(int id);
    }
}
