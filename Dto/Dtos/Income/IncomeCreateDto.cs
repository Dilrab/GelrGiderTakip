using System.ComponentModel.DataAnnotations;

namespace ApiGelirGider.DTOs.Income
{
    public class IncomeCreateDto: ApiGelirGider.DTOs.ResultModel.ResultModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "tipi Boş geçilemez")]
        public decimal IncomeAmount { get; set; } // Girilecek miktar

        [Required(AllowEmptyStrings = false, ErrorMessage = "tipi Boş geçilemez")]
        public int CategoryId { get; set; }    // Kategori FK

        [Required(AllowEmptyStrings = false, ErrorMessage = "tipi Boş geçilemez")]
        public string IncomeSource { get; set; } // Girilecek miktar


        [Required(AllowEmptyStrings = false, ErrorMessage = "tipi Boş geçilemez")]
        public int UserId { get; set; } // ← Bu mutlaka olmalı

        [Required(AllowEmptyStrings = false, ErrorMessage = "tipi Boş geçilemez")]
        public DateTime IncomeDate { get; set; }                // Gelir tarihi
    }
}
