using ApiGelirGider.DTOs.Category;
using ApiGelirGider.DTOs.Income;
using AutoMapper;
using IncomeExpenseTracker.Entities;

namespace ApiGelirGider.WebApi.Mappings
{
    public class MappingProfile : Profile //entity dönüşümü için açıldı
    {
        public MappingProfile()
        {
            CreateMap<Income, IncomeDto>().ReverseMap();
            CreateMap<IncomeDto, Income>().ReverseMap();
            CreateMap<CategoryCreateDto, Category>().ReverseMap();
        }
    }
}
