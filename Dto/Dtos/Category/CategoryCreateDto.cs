using System.ComponentModel.DataAnnotations;

namespace ApiGelirGider.WebApi.DTOs.Category
{
    public class CategoryCreateDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Adı Boş geçilemez")]
        public string CategoryName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "tipi Boş geçilemez")]
        public int Type { get; set; }
    }
}