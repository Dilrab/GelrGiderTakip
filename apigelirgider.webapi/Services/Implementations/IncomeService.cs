// Services/Implementations/IncomeService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGelirGider.DTOs.Income;
using ApiGelirGider.WebApi.Context;
using AutoMapper;
using IncomeExpenseTracker.Entities;
using Microsoft.EntityFrameworkCore;
using ApiGelirGider.Services.Interfaces;

namespace ApiGelirGider.Services.Implementations
{
    public class IncomeService : IIncomeService
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public IncomeService(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<IncomeDto>> GetAllAsync()
        {
            var entities = await _context.Incomes.ToListAsync();
            return _mapper.Map<List<IncomeDto>>(entities);
        }

        public async Task<IncomeDto> GetByIdAsync(int id)
        {
            var entity = await _context.Incomes.FindAsync(id);
            return entity == null
                ? null
                : _mapper.Map<IncomeDto>(entity);
        }

        public async Task AddAsync(IncomeDto dto)
        {
            var entity = _mapper.Map<Income>(dto);
            _context.Incomes.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Incomes.FindAsync(id);
            if (entity != null)
            {
                _context.Incomes.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
