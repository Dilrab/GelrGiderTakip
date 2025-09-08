using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace ApiGelirGider.DTOs.Income
{
    public class IncomeCreateDto : ApiGelirGider.DTOs.ResultModel.ResultModel
    {
        public int IncomeId { get; set; }

        [Required(ErrorMessage = "Tutar zorunludur.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Tutar pozitif olmalıdır.")]
        public decimal IncomeAmount { get; set; }

        [Required(ErrorMessage = "Tarih zorunludur.")]
        public DateTime IncomeDate { get; set; }

        [Required(ErrorMessage = "Kategori seçimi zorunludur.")]
        public int CategoryId { get; set; }

        public int UserId { get; set; } 


        public List<SelectListItem> CategoryList { get; set; } = new();
        public List<IncomeDto> IncomeList { get; set; } = new();

        public string? ResultMessage { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
