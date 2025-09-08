using ApiGelirGider.DTOs.Income;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using ApiGelirGider.DTOs.Expense;

namespace ApiGelirGider.DTOs.Expense
{
    public class ExpenseCreateDto: ApiGelirGider.DTOs.ResultModel.ResultModel
    {
        public int ExpenseId { get; set; }

        [Required(ErrorMessage = "Tutar zorunludur.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Tutar pozitif olmalıdır.")]
        public decimal ExpenseAmount { get; set; }

        [Required(ErrorMessage = "Tarih zorunludur.")]
        public DateTime ExpenseDate { get; set; }

        [Required(ErrorMessage = "Kategori seçimi zorunludur.")]
        public int CategoryId { get; set; }

        public int UserId { get; set; } //  Kullanıcıya aitlik


        public List<SelectListItem> CategoryList { get; set; } = new();
        public List<ExpenseDto> ExpenseList { get; set; } = new();

        public string? ResultMessage { get; set; }
        public string? ErrorMessage { get; set; }



    }
}
