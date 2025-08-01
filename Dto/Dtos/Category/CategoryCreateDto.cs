using System.ComponentModel.DataAnnotations;

namespace ApiGelirGider.DTOs.Category
{
    public class CategoryCreateDto : ApiGelirGider.DTOs.ResultModel.ResultModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Adı Boş geçilemez")]
        public string CategoryName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "tipi Boş geçilemez")]
        public int Type { get; set; }


    }
}