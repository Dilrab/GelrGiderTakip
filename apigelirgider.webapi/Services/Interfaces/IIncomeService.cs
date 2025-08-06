// Services/Interfaces/IIncomeService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGelirGider.DTOs.Income;

namespace ApiGelirGider.Services.Interfaces
{
    public interface IIncomeService
    {
        Task<List<IncomeDto>> GetAllAsync();
        Task<IncomeDto> GetByIdAsync(int id);
        Task AddAsync(IncomeDto dto);
        Task DeleteAsync(int id);
    }
}
