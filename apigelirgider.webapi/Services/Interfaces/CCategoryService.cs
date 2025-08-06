using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGelirGider.DTOs.Category;

namespace ApiGelirGider.Services.Interfaces
{
    public interface CCategoryService
    {
        Task<List<CategoryDtoEdit>> GetAllAsync();
        Task<CategoryDtoEdit> GetByIdAsync(int id);
        Task AddAsync(CategoryDtoEdit dto);
        Task DeleteAsync(int id);
    }
}