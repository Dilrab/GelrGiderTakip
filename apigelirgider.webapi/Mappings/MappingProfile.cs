using ApiGelirGider.DTOs.Category;
using ApiGelirGider.DTOs.Expense;
using ApiGelirGider.DTOs.Income;
using AutoMapper;
using IncomeExpenseTracker.Entities;

namespace ApiGelirGider.WebApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Income
            CreateMap<Income, IncomeDto>().ReverseMap();
            CreateMap<IncomeCreateDto, Income>().ReverseMap();

            // Expense
            CreateMap<Expense, ExpenseDto>().ReverseMap();
            CreateMap<ExpenseCreateDto, Expense>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId)) // 👈 kritik eşleme
                .ReverseMap();

            // Category
            CreateMap<CategoryCreateDto, Category>().ReverseMap();
            CreateMap<Category, CategoryDtoEdit>().ReverseMap();
        }
    }
}

