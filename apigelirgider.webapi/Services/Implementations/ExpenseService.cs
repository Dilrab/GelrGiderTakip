// Services/Implementations/IncomeService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGelirGider.DTOs.Expense;
using ApiGelirGider.WebApi.Context;
using AutoMapper;
using IncomeExpenseTracker.Entities;
using Microsoft.EntityFrameworkCore;
using ApiGelirGider.Services.Interfaces;

namespace ApiGelirGider.Services.Implementations
{
    public class ExpenseService : EExpenseService
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public ExpenseService(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ExpenseDto>> GetAllAsync()
        {
            var entities = await _context.Expenses.ToListAsync();
            return _mapper.Map<List<ExpenseDto>>(entities);
        }

        public async Task<ExpenseDto> GetByIdAsync(int id)
        {
            var entity = await _context.Expenses.FindAsync(id);
            return entity == null
                ? null
                : _mapper.Map<ExpenseDto>(entity);
        }

        public async Task AddAsync(ExpenseDto dto)
        {
            var entity = _mapper.Map<Expense>(dto);
            _context.Expenses.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Expenses.FindAsync(id);
            if (entity != null)
            {
                _context.Expenses.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
