
using ApiGelirGider.WebApi.DTOs.Income;

using AutoMapper;
using IncomeExpenseTracker.Entities;

namespace ApiGelirGider.WebApi.Services
{
    public class IncomeService
    {
        private readonly IMapper _mapper;

        public IncomeService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public IncomeDto GetIncomeDto(Income income)
        {
            return _mapper.Map<IncomeDto>(income);
        }
        public Income ConvertToEntity(IncomeDto dto)
        {
            return _mapper.Map<Income>(dto);
        }

    }
}
