using System.ComponentModel.DataAnnotations;

namespace ApiGelirGider.DTOs.Expense
{
    public class ExpenseCreateDto: ApiGelirGider.DTOs.ResultModel.ResultModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "tipi Boş geçilemez")]
        public decimal ExpenseAmount { get; set; }               // Kullanıcının harcama miktarı

        [Required(AllowEmptyStrings = false, ErrorMessage = "tipi Boş geçilemez")]
        public int CategoryId { get; set; }       // Kategori FK

        [Required(AllowEmptyStrings = false, ErrorMessage = "tipi Boş geçilemez")]
        public int UserId { get; set; } // ← Bu mutlaka olmalı

        [Required(AllowEmptyStrings = false, ErrorMessage = "tipi Boş geçilemez")]
        public DateTime ExpenseDate { get; set; }                // Harcama tarihi


    }
}
