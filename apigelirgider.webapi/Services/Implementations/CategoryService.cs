// Services/Implementations/IncomeService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGelirGider.DTOs.Category;
using ApiGelirGider.WebApi.Context;
using AutoMapper;
using IncomeExpenseTracker.Entities;
using Microsoft.EntityFrameworkCore;
using ApiGelirGider.Services.Interfaces;

namespace ApiGelirGider.Services.Implementations
{
    public class CategoryService : CCategoryService
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public CategoryService(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CategoryDtoEdit>> GetAllAsync()
        {
            var entities = await _context.Categories.ToListAsync();
            return _mapper.Map<List<CategoryDtoEdit>>(entities);
        }

        public async Task<CategoryDtoEdit> GetByIdAsync(int id)
        {
            var entity = await _context.Categories.FindAsync(id);
            return entity == null
                ? null
                : _mapper.Map<CategoryDtoEdit>(entity);
        }

        public async Task AddAsync(CategoryDtoEdit dto)
        {
            var entity = _mapper.Map<Category>(dto);
            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Categories.FindAsync(id);
            if (entity != null)
            {
                _context.Categories.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
