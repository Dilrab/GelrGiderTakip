using ApiGelirGider.WebApi.DTOs.Income;
using IncomeExpenseTracker.Entities;
using AutoMapper;


namespace apigelirgider.webapi.Mappings
{
    public class MappingProfile : Profile //entity dönüşümü için açıldı
    {
        public MappingProfile()
        {
            CreateMap<IncomeCreateDto, Income>();
        }
    }
}
